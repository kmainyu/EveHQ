﻿// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (EveHQMarketDataProvider.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================
namespace EveHQ.Market.MarketServices
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using EveHQ.Caching;
    using EveHQ.Caching.Raven;

    using Newtonsoft.Json;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class EveHqMarketDataProvider : IMarketStatDataProvider
    {
        /// <summary>The eve hq base location.</summary>
        private const string EveHqBaseLocation = "http://market.evehq.net/api/digest/";


        /// <summary>The cache folder.</summary>
        private const string CacheFolder = "PriceCache";

        /// <summary>The data not initialized.</summary>
        private const string DataNotInitialized = "The pricing data cache has not been initialized, or was recently wiped. In order to complete requests for pricing data, a new set of price values must be download. This is a few megabytes of data, so it may take a few moments to complete. Until then pricing queries will return inaccurate values. \n\n Do you wish to download a new set of market data now?";

        private const string NewMarketData = "New market data has been downloaded. Please reload any views containing pricing data to see the latest values.";

        /// <summary>The last download ts.</summary>
        private const string LastDownloadTs = "LastDownloadTime";

        /// <summary>The item key format.</summary>
        private const string ItemKeyFormat = "{0}";

        /// <summary>The Jita system ID.</summary>
        private const int JitaSystemId = 30000142;

        /// <summary>The _cache ttl.</summary>
        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(12);

        /// <summary>The _region data cache.</summary>
        private readonly ICacheProvider _priceCache;

        /// <summary>The _proxy password.</summary>
        private readonly string _proxyPassword;

        /// <summary>The _proxy server address.</summary>
        private readonly Uri _proxyServerAddress;

        /// <summary>The _proxy user name.</summary>
        private readonly string _proxyUserName;

        /// <summary>The _use basic auth.</summary>
        private readonly bool _useBasicAuth;

        /// <summary>The _use default credential.</summary>
        private readonly bool _useDefaultCredential;

        private static object initLockObj = new object();

        private static object downloadLock = new object();

        private static bool downloadInProgres;

        private static ConcurrentDictionary<string, CacheItem<MarketLocationData>> LocationCache = new ConcurrentDictionary<string, CacheItem<MarketLocationData>>();

        /// <summary>Initializes a new instance of the <see cref="EveHqMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        public EveHqMarketDataProvider(string cacheRootFolder)
        {
            _priceCache = new RavenCacheProvider(Path.Combine(cacheRootFolder, CacheFolder));
        }

        /// <summary>Initializes a new instance of the <see cref="EveHqMarketDataProvider"/> class. Initializes a new instance of the <see cref="EveCentralMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        public EveHqMarketDataProvider(string cacheRootFolder, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
            : this(cacheRootFolder)
        {
            _proxyServerAddress = proxyServerAddress;
            _useDefaultCredential = useDefaultCredential;
            _proxyUserName = proxyUserName;
            _proxyPassword = proxyPassword;
            _useBasicAuth = useBasicAuth;
        }

        public string ProviderName
        {
            get
            {
                return Name;
            }
        }

        static public string Name
        {
            get
            {
                return "EveHQ (Bulk Download)";
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

        /// <summary>Gets the supported systems.</summary>
        public IEnumerable<int> SupportedSystems
        {
            get
            {
                return new[] { 30000142 };
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

        /// <summary>Gets the supported regions.</summary>
        public IEnumerable<int> SupportedRegions
        {
            get
            {
                return new[] { 10000002 };
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
            return Task<IEnumerable<ItemOrderStats>>.Factory.StartNew(() => this.RetrievePriceData(typeIds, includeRegions, systemId, minQuantity));
        }

        /// <summary>The retrieve price data.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">The include regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        private IEnumerable<ItemOrderStats> RetrievePriceData(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int? systemId, int minQuantity)
        {
            // check that we've had some download of data in the cache
            CacheItem<DateTimeOffset> lastDownload = _priceCache.Get<DateTimeOffset>(LastDownloadTs);
            if (lastDownload == null)
            {
                // no downloaded data OR someone wiped the cache.
                // we need to alert the user there is no data and that downloading a seed set
                // will take some time. 
                lock (initLockObj)
                {
                    lastDownload = _priceCache.Get<DateTimeOffset>(LastDownloadTs);
                    if (lastDownload == null && !downloadInProgres)
                    {
                        downloadInProgres = true;
                        InitializeDataCache();
                        lastDownload = _priceCache.Get<DateTimeOffset>(LastDownloadTs);
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

            //check memory cache first, then check disk cache

            CacheItem<MarketLocationData> marketData;

            if (!LocationCache.TryGetValue(cacheKey, out marketData))
            {
                marketData = this._priceCache.Get<MarketLocationData>(ItemKeyFormat.FormatInvariant(cacheKey));
                LocationCache.TryAdd(cacheKey, marketData);
            }


            List<ItemOrderStats> cachedResults = new List<ItemOrderStats>();
            if (marketData != null && marketData.Data != null)
            {
                cachedResults.AddRange(typeIds.Select(type => marketData.Data.Items.FirstOrDefault(t => t.ItemTypeId == type)).Where(stats => stats != null));
            }


            // if the lastDownload result is dirty, it's time to queue up a background download of the data.
            if (lastDownload.IsDirty)
            {
                Task.Factory.StartNew(DownloadLatestData);
            }

            return cachedResults;
        }

        /// <summary>The initialize data cache.</summary>
        private void InitializeDataCache()
        {
            // TODO: In EveHQ 3, this must be done by a messaging provider, so we can message the user on the UI from the backside code.
            // For EveHQ 2.x ... we'll ignore SoC to get it working.
            if (MessageBox.Show(DataNotInitialized, "No Market Data!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DownloadLatestData();
            }
        }

        /// <summary>The download latest data.</summary>
        private void DownloadLatestData()
        {
            try
            {
                lock (downloadLock)
                {
                    var lastDownload = _priceCache.Get<DateTimeOffset>(LastDownloadTs);
                    if ( lastDownload == null || lastDownload.IsDirty)
                    {
                        // get the latest system data
                        foreach (int system in SupportedSystems)
                        {
                            var locationData = DownloadData(system);

                            _priceCache.Add(ItemKeyFormat.FormatInvariant(system), locationData, DateTimeOffset.Now.Add(_cacheTtl));

                        }

                        // get the latest region data
                        // get the latest system data
                        foreach (int region in SupportedRegions)
                        {
                            var locationData = DownloadData(region);

                            _priceCache.Add(ItemKeyFormat.FormatInvariant(region), locationData, DateTimeOffset.Now.Add(_cacheTtl));

                        }

                        _priceCache.Add(LastDownloadTs, DateTimeOffset.Now, DateTimeOffset.Now.Add(_cacheTtl));

                        MessageBox.Show(NewMarketData, string.Empty, MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception e)
            {
                // log it.
                throw e;
            }
        }

        /// <summary>The download data.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        /// <exception cref="Exception"></exception>
        private MarketLocationData DownloadData(int entityId)
        {
            MarketLocationData results = null;
            Task<HttpResponseMessage> requestTask = WebRequestHelper.GetAsync(
                new Uri(EveHqBaseLocation + entityId.ToInvariantString()), _proxyServerAddress, _useDefaultCredential, _proxyUserName, _proxyPassword, _useBasicAuth);
            requestTask.Wait(); // wait for the completion (we're in a background task anyways)

            if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
            {

                try
                {
                    var readTask = requestTask.Result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    // process result
                    var retrievedData = JsonConvert.DeserializeObject<MarketLocationData>(readTask.Result);

                    results = retrievedData;
                }
                catch (Exception ex)
                {
                    // todo: log deserialize error.
                    throw ex;
                }

            }

            return results;
        }
    }
}