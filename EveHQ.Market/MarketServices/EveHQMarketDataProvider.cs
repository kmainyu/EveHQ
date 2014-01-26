// ===========================================================================
// <copyright file="EveHQMarketDataProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (EveHQMarketDataProvider.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.Market.MarketServices
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;
    using EveHQ.Market.Properties;

    using Ionic.Zip;

    using Newtonsoft.Json;

    /// <summary>
    ///     Market data provider, utilizing the EveHQ Market Data Service.
    /// </summary>
    public class EveHQMarketDataProvider : IMarketStatDataProvider
    {
        /// <summary>The cache folder.</summary>
        private const string CacheFolder = "PriceCache";

        /// <summary>The eve hq base location.</summary>
        private const string EveHqBaseLocation = "http://market.evehq.net/api/digest/";

        /// <summary>The eve hq market data dumps location.</summary>
        private const string EveHqMarketDataDumpsLocation = "http://market.evehq.net/dumps/{0}.zip";

        /// <summary>The item key format.</summary>
        private const string ItemKeyFormat = "{0}";

        /// <summary>The Jita system ID.</summary>
        private const int JitaSystemId = 30000142;

        /// <summary>The last download ts.</summary>
        private const string LastDownloadTs = "LastDownloadTime- {0}";

        /// <summary>The download lock.</summary>
        private static readonly object DownloadLock = new object();

        /// <summary>The init lock obj.</summary>
        private static readonly object InitLockObj = new object();

        /// <summary>The location cache.</summary>
        private static readonly ConcurrentDictionary<string, CacheItem<IEnumerable<ItemOrderStats>>> LocationCache = new ConcurrentDictionary<string, CacheItem<IEnumerable<ItemOrderStats>>>();

        /// <summary>The download in progres.</summary>
        private static bool downloadInProgres;

        /// <summary>The _cache ttl.</summary>
        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(12);

        /// <summary>The _region data cache.</summary>
        private readonly ICacheProvider _priceCache;

        /// <summary>The _request provider.</summary>
        private readonly IHttpRequestProvider _requestProvider;

        /// <summary>Initializes a new instance of the <see cref="EveHQMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        /// <param name="requestProvider">The request Provider.</param>
        public EveHQMarketDataProvider(string cacheRootFolder, IHttpRequestProvider requestProvider)
        {
            _priceCache = new TextFileCacheProvider(Path.Combine(cacheRootFolder, CacheFolder));
            _requestProvider = requestProvider;
        }

        /// <summary>Gets the name.</summary>
        public static string Name
        {
            get
            {
                return "EveHQ (Bulk Download)";
            }
        }

        /// <summary>Gets a value indicating whether limited region selection.</summary>
        public bool LimitedRegionSelection
        {
            get
            {
                return true;
            }
        }

        /// <summary>Gets a value indicating whether limited system selection.</summary>
        public bool LimitedSystemSelection
        {
            get
            {
                return true;
            }
        }

        /// <summary>Gets the provider name.</summary>
        public string ProviderName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>Gets the supported regions.</summary>
        public IEnumerable<int> SupportedRegions
        {
            get
            {
                return new[] { 10000002 };
            }
        }

        /// <summary>Gets the supported systems.</summary>
        public IEnumerable<int> SupportedSystems
        {
            get
            {
                return new[] { 30000142 };
            }
        }

        /// <summary>The get order stats.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">The include regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<IEnumerable<ItemOrderStats>> GetOrderStats(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int? systemId, int minQuantity)
        {
            return Task<IEnumerable<ItemOrderStats>>.Factory.TryRun(() => RetrievePriceData(typeIds, includeRegions, systemId));
        }

        /// <summary>The download data.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        private IEnumerable<ItemOrderStats> DownloadData(int entityId)
        {
            IEnumerable<ItemOrderStats> results = null;
            Task<HttpResponseMessage> requestTask = _requestProvider.GetAsync(new Uri(EveHqMarketDataDumpsLocation.FormatInvariant(entityId.ToInvariantString())));
            requestTask.Wait(); // wait for the completion (we're in a background task anyways)

            if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
            {
                Task<Stream> readTask = requestTask.Result.Content.ReadAsStreamAsync();
                readTask.Wait();

                // process result
                using (var buffer = new MemoryStream())
                using (ZipFile compressedData = ZipFile.Read(readTask.Result))
                {
                    ZipEntry marketData = compressedData["{0}.txt".FormatInvariant(entityId)];
                    marketData.Extract(buffer);

                    string jsonData = Encoding.UTF8.GetString(buffer.ToArray());
                    results = JsonConvert.DeserializeObject<IEnumerable<ItemOrderStats>>(jsonData);
                }
            }

            return results;
        }

        /// <summary>The download latest data.</summary>
        /// <param name="marketLocation">The market Location.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "EveHQ is not globalized yet.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Caught for logging purposes, and so the user can be alerted for retry.")]
        private void DownloadLatestData(int marketLocation)
        {
            bool retry = false;
            lock (DownloadLock)
            {
                try
                {
                    string lastDownloadKey = LastDownloadTs.FormatInvariant(marketLocation);
                    CacheItem<DateTimeOffset> lastDownload = _priceCache.Get<DateTimeOffset>(lastDownloadKey);

                    // check to see if there is newer data available.
                    MarketLocationData marketDataInfo = LastMarketUpdate(marketLocation);

                    if (lastDownload == null || (lastDownload.IsDirty && lastDownload.Data < marketDataInfo.Freshness))
                    {
                        IEnumerable<ItemOrderStats> locationData = DownloadData(marketLocation);

                        _priceCache.Add(ItemKeyFormat.FormatInvariant(marketLocation), locationData, DateTimeOffset.Now.Add(_cacheTtl));

                        MessageBox.Show(Resources.NewMarketData, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _priceCache.Add(lastDownloadKey, DateTimeOffset.Now, DateTimeOffset.Now.Add(_cacheTtl));
                    }
                }
                catch (Exception e)
                {
                    // log it.
                    Trace.TraceError(e.FormatException());
                    if (MessageBox.Show(Resources.ErrorDownloadingData, Resources.ErrorCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        retry = true;
                    }
                    else
                    {
                        string lastDownloadKey = LastDownloadTs.FormatInvariant(marketLocation);
                        _priceCache.Add(lastDownloadKey, DateTimeOffset.Now, DateTimeOffset.Now.AddMinutes(30));
                    }
                }
            }

            if (retry)
            {
                DownloadLatestData(marketLocation);
            }
        }

        /// <summary>The initialize data cache.</summary>
        /// <param name="marketLocation">The market Location.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "EveHQ is not globalized yet.")]
        private void InitializeDataCache(int marketLocation)
        {
            // TODO: In EveHQ 3, this must be done by a messaging provider, so we can message the user on the UI from the backside code.
            // For EveHQ 2.x ... we'll ignore SoC to get it working.
            if (MessageBox.Show(Resources.DataNotInitialized, Resources.NoMarketDataCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DownloadLatestData(marketLocation);
            }
        }

        /// <summary>The last market update.</summary>
        /// <param name="marketLocationId">The market location id.</param>
        /// <returns>The <see cref="MarketLocationData"/>.</returns>
        private MarketLocationData LastMarketUpdate(int marketLocationId)
        {
            Task<HttpResponseMessage> requestTask = _requestProvider.GetAsync(new Uri(EveHqBaseLocation + marketLocationId.ToInvariantString()), "application/json");

            requestTask.Wait();
            MarketLocationData results = null;
            if (requestTask.IsCompleted && requestTask.Result != null && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
            {
                Task<string> readTask = requestTask.Result.Content.ReadAsStringAsync();
                readTask.Wait();

                results = JsonConvert.DeserializeObject<MarketLocationData>(readTask.Result);
            }

            return results;
        }

        /// <summary>The retrieve price data.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">The include regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        private IEnumerable<ItemOrderStats> RetrievePriceData(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int? systemId)
        {
            // check that we've had some download of data in the cache
            int marketLocation = systemId.HasValue ? systemId.Value : includeRegions != null ? includeRegions.FirstOrDefault() : 0;
            string lastDownloadKey = LastDownloadTs.FormatInvariant(marketLocation);
            CacheItem<DateTimeOffset> lastDownload = _priceCache.Get<DateTimeOffset>(lastDownloadKey);
            if (lastDownload == null)
            {
                // no downloaded data OR someone wiped the cache.
                // we need to alert the user there is no data and that downloading a seed set
                // will take some time. 
                lock (InitLockObj)
                {
                    lastDownload = _priceCache.Get<DateTimeOffset>(lastDownloadKey);
                    if (lastDownload == null && !downloadInProgres)
                    {
                        downloadInProgres = true;
                        InitializeDataCache(marketLocation);
                        lastDownload = _priceCache.Get<DateTimeOffset>(lastDownloadKey);
                        downloadInProgres = false;
                    }
                }
            }

            string cacheKey;
            if (systemId.HasValue)
            {
                cacheKey = systemId.Value.ToInvariantString();
            }
            else if (includeRegions != null && includeRegions.Any())
            {
                cacheKey = includeRegions.First().ToInvariantString();
            }
            else
            {
                cacheKey = JitaSystemId.ToInvariantString();
            }

            // check memory cache first, then check disk cache
            CacheItem<IEnumerable<ItemOrderStats>> marketData;

            if (!LocationCache.TryGetValue(cacheKey, out marketData))
            {
                marketData = _priceCache.Get<IEnumerable<ItemOrderStats>>(ItemKeyFormat.FormatInvariant(cacheKey));
                LocationCache.TryAdd(cacheKey, marketData);
            }

            var cachedResults = new List<ItemOrderStats>();
            if (marketData != null && marketData.Data != null)
            {
                cachedResults.AddRange(typeIds.Select(type => marketData.Data.FirstOrDefault(t => t.ItemTypeId == type)).Where(stats => stats != null));
            }

            // if the lastDownload result is dirty, it's time to queue up a background download of the data.
            if (lastDownload != null && lastDownload.IsDirty)
            {
                Task.Factory.TryRun(() => DownloadLatestData(marketLocation));
            }

            return cachedResults;
        }
    }
}