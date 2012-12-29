// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (EveCentralMarketDataProvider.cs), is part of EveHQ.
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
namespace EveHQ.Market
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Market data provider, sourcing data from Eve-Central.com
    /// </summary>
    public class EveCentralMarketDataProvider : IMarketDataProvider
    {
        /// <summary>The region.</summary>
        private const string Region = "Region";

        /// <summary>The system.</summary>
        private const string System = "System";

        /// <summary>The item key format.</summary>
        private const string ItemKeyFormat = "{0}_{1}";

        /// <summary>The eve central base url.</summary>
        private const string EveCentralBaseUrl = "http://api.eve-central.com/api/";

        /// <summary>The market stat api.</summary>
        private const string MarketStatAPI = "marketstat";

        /// <summary>The type id.</summary>
        private const string TypeId = "typeid";

        /// <summary>The region limit.</summary>
        private const string RegionLimit = "regionlimit";

        /// <summary>The use system.</summary>
        private const string UseSystem = "usesystem";

        /// <summary>The min quantity.</summary>
        private const string MinQuantity = "minQ";

        /// <summary>The query param format.</summary>
        private const string QueryParamFormat = "{0}{1}={2}";

        /// <summary>The amp.</summary>
        private const string Amp = "&";

        /// <summary>The question.</summary>
        private const string Question = "?";

        /// <summary>The _region data cache.</summary>
        private readonly SimpleTextFileCache _regionDataCache;

        /// <summary>The _system data cache.</summary>
        private readonly SimpleTextFileCache _systemDataCache;

        /// <summary>Initializes a new instance of the <see cref="EveCentralMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        public EveCentralMarketDataProvider(string cacheRootFolder)
        {
            this._regionDataCache = new SimpleTextFileCache(Path.Combine(cacheRootFolder, Region));
            this._systemDataCache = new SimpleTextFileCache(Path.Combine(cacheRootFolder, System));
        }

        /// <summary>The get region based order stats.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">The include regions.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">Thrown when there's an XML exception</exception>
        /// <exception cref="InvalidOperationException">Thrown when there was a problem with the web request.</exception>
        public Task<IEnumerable<ItemOrderStats>> GetRegionBasedOrderStats(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int minQuantity)
        {
            return Task<IEnumerable<ItemOrderStats>>.Factory.StartNew(
                () =>
                {
                    var cachedItems = new List<ItemOrderStats>();
                    var typesToRequest = new List<int>();
                    string cacheKey = this.CalcCacheKey(includeRegions);
                    foreach (int typeId in typeIds.Distinct())
                    {
                        var itemStats = this._regionDataCache.Get<ItemOrderStats>(ItemKeyFormat.FormatInvariant(typeId, cacheKey));
                        if (itemStats != null)
                        {
                            cachedItems.Add(itemStats);
                        }
                        else
                        {
                            typesToRequest.Add(typeId);
                        }
                    }

                    if (typesToRequest.Any())
                    {
                        // make the request for the types we don't have valid caches for
                        Uri requestUri = this.CreateMarketStatRequest(typesToRequest, includeRegions, 0, minQuantity);

                        Task<WebResponse> requestTask = this.RequestAsync(requestUri);
                        requestTask.Wait(); // wait for the completion (we're in a background task anyways)

                        if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
                        {
                            using (Stream stream = requestTask.Result.GetResponseStream())
                            {
                                try
                                {
                                    // process result
                                    List<ItemOrderStats> retrievedItems = this.GetOrderStatsFromXml(stream);

                                    // cache it.
                                    foreach (ItemOrderStats item in retrievedItems)
                                    {
                                        this._regionDataCache.Add(ItemKeyFormat.FormatInvariant(item.ItemTypeId.ToInvariantString(), cacheKey), item, DateTimeOffset.Now.Add(TimeSpan.FromHours(1)));
                                    }

                                    // add them to the cached item collection for return
                                    cachedItems.AddRange(retrievedItems);
                                }
                                catch (Exception ex)
                                {
                                    // todo: log/report XML parsing error.
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("There was a problem requesting the market data.", requestTask.Exception);
                        }
                    }

                    return cachedItems;
                });
        }

        /// <summary>The get order stats by system.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<IEnumerable<ItemOrderStats>> GetOrderStatsBySystem(IEnumerable<int> typeIds, int systemId, int minQuantity)
        {
            return Task<IEnumerable<ItemOrderStats>>.Factory.StartNew(
              () =>
              {
                  var cachedItems = new List<ItemOrderStats>();
                  var typesToRequest = new List<int>();
                  string cacheKey = this.CalcCacheKey(new[] { systemId });
                  foreach (int typeId in typeIds.Distinct())
                  {
                      var itemStats = this._systemDataCache.Get<ItemOrderStats>(ItemKeyFormat.FormatInvariant(typeId, cacheKey));
                      if (itemStats != null)
                      {
                          cachedItems.Add(itemStats);
                      }
                      else
                      {
                          typesToRequest.Add(typeId);
                      }
                  }

                  if (typesToRequest.Any())
                  {
                      // make the request for the types we don't have valid caches for
                      Uri requestUri = this.CreateMarketStatRequest(typesToRequest, null, systemId, minQuantity);

                      Task<WebResponse> requestTask = this.RequestAsync(requestUri);
                      requestTask.Wait(); // wait for the completion (we're in a background task anyways)

                      if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
                      {
                          using (Stream stream = requestTask.Result.GetResponseStream())
                          {
                              try
                              {
                                  // process result
                                  List<ItemOrderStats> retrievedItems = this.GetOrderStatsFromXml(stream);

                                  // cache it.
                                  foreach (ItemOrderStats item in retrievedItems)
                                  {
                                      this._systemDataCache.Add(ItemKeyFormat.FormatInvariant(item.ItemTypeId.ToInvariantString(), cacheKey), item, DateTimeOffset.Now.Add(TimeSpan.FromHours(1)));
                                  }

                                  // add them to the cached item collection for return
                                  cachedItems.AddRange(retrievedItems);
                              }
                              catch (Exception ex)
                              {
                                  // todo: log/report XML parsing error.
                                  throw ex;
                              }
                          }
                      }
                      else
                      {
                          throw new InvalidOperationException("There was a problem requesting the market data.", requestTask.Exception);
                      }
                  }

                  return cachedItems;
              });
        }

        /// <summary>The get order stats from xml.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The <see cref="List"/>.</returns>
        private List<ItemOrderStats> GetOrderStatsFromXml(Stream stream)
        {
            List<ItemOrderStats> orders = null;
            using (TextReader reader = new StreamReader(stream))
            {
                XDocument xml = XDocument.Load(reader);
                XElement statNode;
                if (xml.Root != null && (statNode = xml.Root.Element("marketstat")) != null)
                {
                    orders = (from stats in xml.Root.Elements("marketstat")
                              from type in stats.Elements("type")
                              let typeId = type.Attribute("id").Value.ToInt()
                              let buyData = this.GetOrderData(type.Element("buy"))
                              let sellData = this.GetOrderData(type.Element("sell"))
                              let allData = this.GetOrderData(type.Element("all"))
                              select new ItemOrderStats() { ItemTypeId = typeId, Buy = buyData, Sell = sellData, All = allData }).ToList();
                }
            }

            return orders;
        }

        /// <summary>The get order data.</summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="OrderStats"/>.</returns>
        private OrderStats GetOrderData(XElement element)
        {
            // ReSharper disable PossibleNullReferenceException
            // null exceptions are caught further up the call stack.
            long volume = element.Element("volume").Value.ToLong();

            double avg = element.Element("avg").Value.ToDouble();
            double max = element.Element("max").Value.ToDouble();
            double min = element.Element("min").Value.ToDouble();
            double stddev = element.Element("stddev").Value.ToDouble();
            double median = element.Element("median").Value.ToDouble();
            double percentile = element.Element("percentile").Value.ToDouble();

            return new OrderStats() { Volume = volume, Average = avg, Maximum = max, Minimum = min, StdDeviation = stddev, Median = median, Percentile = percentile };
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>The create market stat request.</summary>
        /// <param name="typesToRequest">The types to request.</param>
        /// <param name="regions">The regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        private Uri CreateMarketStatRequest(IEnumerable<int> typesToRequest, IEnumerable<int> regions, int systemId, int minQuantity)
        {
            var uri = new UriBuilder(string.Concat(EveCentralBaseUrl, MarketStatAPI));
            var query = new StringBuilder();
            foreach (int type in typesToRequest)
            {
                query.AppendFormat(QueryParamFormat, query.Length > 0 ? Amp : string.Empty, TypeId, type);
            }

            if (systemId == 0)
            {
                foreach (int region in regions)
                {
                    query.AppendFormat(QueryParamFormat, query.Length > 0 ? Amp : string.Empty, RegionLimit, region);
                }
            }
            else
            {
                query.AppendFormat(QueryParamFormat, query.Length > 0 ? Amp : string.Empty, UseSystem, systemId);
            }

            if (minQuantity > 0)
            {
                query.AppendFormat(QueryParamFormat, query.Length > 0 ? Amp : string.Empty, MinQuantity, minQuantity);
            }

            uri.Query = query.ToString();
            return uri.Uri;
        }

        /// <summary>Calculates the cache key to use.</summary>
        /// <param name="ids">The ids.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string CalcCacheKey(IEnumerable<int> ids)
        {
            return string.Join(string.Empty, ids.Select(id => id.ToInvariantString()).ToArray()).GetHashCode().ToInvariantString();
        }

        /// <summary>The request async.</summary>
        /// <param name="target">The target.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private Task<WebResponse> RequestAsync(Uri target)
        {
            var request = WebRequest.Create(target) as HttpWebRequest;
            

            return Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
        }
    }
}