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
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml.Linq;

    /// <summary>
    ///     Market data provider, sourcing data from Eve-Central.com
    /// </summary>
    public class EveCentralMarketDataProvider : IMarketDataProvider, IMarketDataReceiver
    {
        /// <summary>The region.</summary>
        private const string Region = "Region";

        /// <summary>The system.</summary>
        private const string System = "System";

        /// <summary>The item key format.</summary>
        private const string ItemKeyFormat = "{0}";

        /// <summary>The eve central base url.</summary>
        private const string EveCentralBaseUrl = "http://api.eve-central.com/api/";

        /// <summary>The market stat API.</summary>
        private const string MarketStatApi = "marketstat";

        /// <summary>
        /// quicklook (detailed) API
        /// </summary>
        private const string QuickLookApi = "quicklook";

        /// <summary>The upload API.</summary>
        private const string UploadApi = "upload";

        /// <summary>The name of the parameter to set UDF data to.</summary>
        private const string UdfPostParameterName = "data";

        /// <summary>The type id.</summary>
        private const string TypeId = "typeid";

        /// <summary>The region limit.</summary>
        private const string RegionLimit = "regionlimit";

        /// <summary>The use system.</summary>
        private const string UseSystem = "usesystem";

        /// <summary>The min quantity.</summary>
        private const string MinQuantity = "minQ";

        /// <summary>The Jita system ID.</summary>
        private const int JitaSystemId = 30000142;

        /// <summary>The _market order cache.</summary>
        private readonly SimpleTextFileCache _marketOrderCache;

        /// <summary>The _region data cache.</summary>
        private readonly SimpleTextFileCache _regionDataCache;

        /// <summary>The _upload key.</summary>
        private readonly UploadKey _uploadKey = new UploadKey { Key = "0", Name = "Eve-Central" };

        /// <summary>The _next upload attempt.</summary>
        private DateTimeOffset _nextUploadAttempt;

        /// <summary>The _upload service online.</summary>
        private bool _uploadServiceOnline;

        /// <summary>Initializes a new instance of the <see cref="EveCentralMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        public EveCentralMarketDataProvider(string cacheRootFolder)
        {
            _regionDataCache = new SimpleTextFileCache(Path.Combine(cacheRootFolder, Region));
            _marketOrderCache = new SimpleTextFileCache(Path.Combine(cacheRootFolder, "MarketOrders"));
        }

        /// <summary>Initializes a new instance of the <see cref="EveCentralMarketDataProvider"/> class.</summary>
        internal EveCentralMarketDataProvider()
        {
        }

        /// <summary>The get region based order stats.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">Include these regions.</param>
        /// <param name="systemId">Use this system id to source the data (overrides region values).</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="Exception">Thrown when there's an XML exception</exception>
        /// <exception cref="InvalidOperationException">Thrown when there was a problem with the web request.</exception>
        public Task<IEnumerable<ItemOrderStats>> GetOrderStats(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int? systemId, int minQuantity)
        {
            return Task<IEnumerable<ItemOrderStats>>.Factory.StartNew(
                () =>
                {
                    var cachedItems = new List<ItemOrderStats>();
                    var typesToRequest = new List<int>();
                    string cacheKey;

                    if (systemId.HasValue)
                    {
                        cacheKey = this.CalcCacheKey(new[] { systemId.Value });
                    }
                    else if (includeRegions != null && includeRegions.Any())
                    {
                        cacheKey = this.CalcCacheKey(includeRegions);
                    }
                    else
                    {
                        cacheKey = this.CalcCacheKey(new[] { JitaSystemId });
                    }

                    foreach (int typeId in typeIds.Distinct())
                    {
                        var itemStats = this._regionDataCache.Get<List<ItemOrderStats>>(ItemKeyFormat.FormatInvariant(cacheKey));
                        ItemOrderStats itemData;
                        if (itemStats != null &&  (itemData = itemStats.FirstOrDefault(i => i.ItemTypeId == typeId)) != null)
                        {
                            cachedItems.Add(itemData);
                        }
                        else
                        {
                            typesToRequest.Add(typeId);
                        }
                    }

                    if (typesToRequest.Any())
                    {
                        // make the request for the types we don't have valid caches for
                        NameValueCollection requestParameters = this.CreateMarketRequestParameters(typesToRequest, includeRegions, systemId ?? 0, minQuantity);

                        Task<WebResponse> requestTask = WebRequestHelper.PostAsync(new Uri(EveCentralBaseUrl + MarketStatApi), requestParameters);
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
                                    var existingData = _regionDataCache.Get<List<ItemOrderStats>>(cacheKey);
                                    if (existingData != null)
                                    {
                                        existingData.AddRange(retrievedItems);
                                    }
                                    else
                                    {
                                        existingData = retrievedItems;
                                    }

                                    _regionDataCache.Add(cacheKey, existingData, DateTimeOffset.Now.Add(TimeSpan.FromHours(1)));

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

        /// <summary>Gets the current market orders for the item, using the region, system and quantity constraints</summary>
        /// <param name="itemTypeId">The item ID to query</param>
        /// <param name="includedRegions">Regions to include in the query for data.</param>
        /// <param name="systemId">Specific system to use to source data (if set will overrride regions)</param>
        /// <param name="minQuantity">Minimum Quantity for an order to be included/</param>
        /// <returns>Returns a reference to the async task.</returns>
        public Task<ItemMarketOrders> GetMarketOrdersForItemType(int itemTypeId, IEnumerable<int> includedRegions, int? systemId, int minQuantity)
        {
            return Task<ItemMarketOrders>.Factory.StartNew(
                () =>
                {
                    string cacheKey;
                    if (systemId.HasValue)
                    {
                        cacheKey = this.CalcCacheKey(new[] { systemId.Value });
                    }
                    else if (includedRegions != null && includedRegions.Any())
                    {
                        cacheKey = this.CalcCacheKey(includedRegions);
                    }
                    else
                    {
                        cacheKey = this.CalcCacheKey(new[] { JitaSystemId });
                    }

                    var resultData = _marketOrderCache.Get<ItemMarketOrders>(ItemKeyFormat.FormatInvariant(itemTypeId, cacheKey));
                    if (resultData == null)
                    {
                        // make the request for the types we don't have valid caches for
                        NameValueCollection requestParameters = this.CreateMarketRequestParameters(new[] { itemTypeId }, includedRegions, systemId ?? 0, minQuantity);
                        Task<WebResponse> requestTask = WebRequestHelper.PostAsync(new Uri(EveCentralBaseUrl + QuickLookApi), requestParameters);
                        requestTask.Wait(); // wait for the completion (we're in a background task anyways)

                        if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
                        {
                            using (Stream stream = requestTask.Result.GetResponseStream())
                            {
                                try
                                {
                                    // process result
                                    ItemMarketOrders data = this.GetMarketOrdersFromXml(stream);

                                    _marketOrderCache.Add(ItemKeyFormat.FormatInvariant(itemTypeId, cacheKey), data, DateTimeOffset.Now.Add(TimeSpan.FromHours(1)));

                                    resultData = data;
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

                    return resultData;
                });
        }

        /// <summary>Gets a value indicating whether is enabled.</summary>
        public bool IsEnabled
        {
            get
            {
                return _uploadServiceOnline;
            }
        }

        /// <summary>Gets the next attempt.</summary>
        public DateTimeOffset NextAttempt
        {
            get
            {
                return _nextUploadAttempt;
            }
        }

        /// <summary>Gets the unified upload key.</summary>
        public UploadKey UnifiedUploadKey
        {
            get
            {
                return _uploadKey;
            }
        }

        /// <summary>Attempts to upload the data values to the receiver</summary>
        /// <param name="marketDataJson">The market data JSON string.</param>
        /// <returns>A reference to the Async Task.</returns>
        public Task UploadMarketData(string marketDataJson)
        {
            // creat the URL for the request
            var requestUri = new Uri(EveCentralBaseUrl + UploadApi);
            var paramData = new NameValueCollection { { UdfPostParameterName, HttpUtility.UrlEncode(marketDataJson) } };

            // send the request and return the task handle after checking the return of the web request
            return WebRequestHelper.PostAsync(requestUri, paramData).ContinueWith(
                task =>
                {
                    HttpWebResponse httpResponse;
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted && task.Exception == null && task.Result != null && (httpResponse = task.Result as HttpWebResponse) != null
                        && httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // success
                        _uploadServiceOnline = true;
                        _nextUploadAttempt = DateTimeOffset.Now;
                    }
                    else
                    {
                        // there was something wrong... disable this receiver for a while.
                        _uploadServiceOnline = false;
                        _nextUploadAttempt = DateTimeOffset.Now.AddHours(1);
                    }

                    return task;
                });
        }

        /// <summary>The get order stats from xml.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The market orders object.</returns>
        private ItemMarketOrders GetMarketOrdersFromXml(Stream stream)
        {
            var orderData = new ItemMarketOrders();

            using (TextReader reader = new StreamReader(stream))
            {
                XDocument xml = XDocument.Load(reader);

                // ReSharper disable PossibleNullReferenceException
                XElement dataElement = xml.Root.Element("quicklook");

                if (dataElement != null)
                {
                    orderData.ItemTypeId = dataElement.Element("item").Value.ToInt();
                    orderData.ItemName = dataElement.Element("itemname").Value;
                    orderData.Regions = new HashSet<int>(dataElement.Element("regions").Value.Split(',').Select(region => region.ToInt()).Where(region => region > 0));
                    orderData.Hours = dataElement.Element("hours").Value.ToInt();
                    orderData.MinQuantity = dataElement.Element("minqty").Value.ToInt();

                    // sell orders
                    XElement orderElement;
                    if ((orderElement = dataElement.Element("sell_orders")) != null)
                    {
                        orderData.SellOrders = GetIndivdualOrders(orderElement, false).ToList();
                    }

                    // buy orders
                    if ((orderElement = dataElement.Element("buy_orders")) != null)
                    {
                        orderData.BuyOrders = GetIndivdualOrders(orderElement, true).ToList();
                    }
                }
            }

            // ReSharper restore PossibleNullReferenceException
            return orderData;
        }

        /// <summary>Processes the order xml into individual order objects</summary>
        /// <param name="orderElement">The order element.</param>
        /// <param name="isBuyOrder">The is buy order.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        private static IEnumerable<MarketOrder> GetIndivdualOrders(XElement orderElement, bool isBuyOrder)
        {
            // ReSharper disable PossibleNullReferenceException
            // The format schema is strict and any exception is caught upstream.
            return from order in orderElement.Elements("order")
                   let orderId = order.Attribute("id").Value.ToLong()
                   let regionId = order.Element("region").Value.ToInt()
                   let stationId = order.Element("station").Value.ToInt()
                   let stationName = order.Element("station_name").Value
                   let security = order.Element("security").Value.ToDouble()
                   let range = order.Element("range").Value.ToInt()
                   let price = order.Element("price").Value.ToDouble()
                   let quantityRemaining = order.Element("vol_remain").Value.ToInt()
                   let minQuantity = order.Element("min_volume").Value.ToInt()
                   let expires = order.Element("expires").Value.ToDateTimeOffset(0)
                   let reported = order.Element("reported_time").Value.ToDateTimeOffset(0)
                   select
                       new MarketOrder
                       {
                           OrderId = orderId, 
                           RegionId = regionId, 
                           StationId = stationId, 
                           StationName = stationName, 
                           Security = security, 
                           OrderRange = range, 
                           Price = price, 
                           QuantityRemaining = quantityRemaining, 
                           MinQuantity = minQuantity, 
                           Expires = expires, 
                           Freshness = reported, 
                           IsBuyOrder = isBuyOrder
                       };

            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>The get order stats from xml.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The collection of item order stats.</returns>
        private List<ItemOrderStats> GetOrderStatsFromXml(Stream stream)
        {
            List<ItemOrderStats> orderStats = null;
            using (TextReader reader = new StreamReader(stream))
            {
                // ReSharper disable PossibleNullReferenceException
                XDocument xml = XDocument.Load(reader);
                if (xml.Root != null && xml.Root.Element("marketstat") != null)
                {
                    orderStats = (from stats in xml.Root.Elements("marketstat")
                                  from type in stats.Elements("type")
                                  let typeId = type.Attribute("id").Value.ToInt()
                                  let buyData = this.GetOrderData(type.Element("buy"))
                                  let sellData = this.GetOrderData(type.Element("sell"))
                                  let allData = this.GetOrderData(type.Element("all"))
                                  select new ItemOrderStats { ItemTypeId = typeId, Buy = buyData, Sell = sellData, All = allData }).ToList();
                }

                // ReSharper restore PossibleNullReferenceException
            }

            return orderStats;
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

            return new OrderStats { Volume = volume, Average = avg, Maximum = max, Minimum = min, StdDeviation = stddev, Median = median, Percentile = percentile };

            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>The create market stat request.</summary>
        /// <param name="typesToRequest">The types to request.</param>
        /// <param name="regions">The regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        private NameValueCollection CreateMarketRequestParameters(IEnumerable<int> typesToRequest, IEnumerable<int> regions, int systemId, int minQuantity)
        {
            var parameters = new NameValueCollection();

            foreach (int type in typesToRequest)
            {
                parameters.Add(TypeId, type.ToInvariantString());
            }

            if (systemId == 0)
            {
                foreach (int region in regions)
                {
                    parameters.Add(RegionLimit, region.ToInvariantString());
                }
            }
            else
            {
                parameters.Add(UseSystem, systemId.ToInvariantString());
            }

            if (minQuantity > 1)
            {
                parameters.Add(MinQuantity, minQuantity.ToInvariantString());
            }

            return parameters;
        }

        /// <summary>Calculates the cache key to use.</summary>
        /// <param name="ids">The ids.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string CalcCacheKey(IEnumerable<int> ids)
        {
            return string.Join(string.Empty, ids.Select(id => id.ToInvariantString()).ToArray()).GetHashCode().ToInvariantString();
        }
    }
}