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

    using EveHQ.Caching;
    using EveHQ.Caching.Raven;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>
    ///     Market data provider, sourcing data from Eve-Central.com
    /// </summary>
    public class EveCentralMarketDataProvider : IMarketStatDataProvider, IMarketDataReceiver, IMarketOrderDataProvider
    {
        /// <summary>The region.</summary>
        private const string Region = "Region";

        /// <summary>The system.</summary>
        private const string System = "System";

        /// <summary>The item key format.</summary>
        private const string ItemKeyFormat = "{0}-{1}";

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
        private readonly ICacheProvider _marketOrderCache;

        /// <summary>The _region data cache.</summary>
        private readonly ICacheProvider _regionDataCache;

        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(1);

        /// <summary>The _upload key.</summary>
        private readonly UploadKey _uploadKey = new UploadKey { Key = "0", Name = "Eve-Central" };

        /// <summary>The _next upload attempt.</summary>
        private DateTimeOffset _nextUploadAttempt;

        /// <summary>The _upload service online.</summary>
        private bool _uploadServiceOnline;

        private Uri _proxyServerAddress;

        private bool _useDefaultCredential;

        private string _proxyUserName;

        private string _proxyPassword;

        private bool _useBasicAuth;

        public EveCentralMarketDataProvider(string cacheRootFolder)
        {
            _regionDataCache = new RavenCacheProvider(Path.Combine(cacheRootFolder, Region));
            _marketOrderCache = new RavenCacheProvider(Path.Combine(cacheRootFolder, "MarketOrders"));

        }

        /// <summary>Initializes a new instance of the <see cref="EveCentralMarketDataProvider"/> class.</summary>
        /// <param name="cacheRootFolder">The cache root folder.</param>
        public EveCentralMarketDataProvider(string cacheRootFolder, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
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
                return "Eve-Central (Live Queries)";
            }
        }

        public bool LimitedSystemSelection
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<int> SupportedSystems
        {
            get
            {
                return new int[0];
            }
        }

        public bool LimitedRegionSelection
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<int> SupportedRegions
        {
            get
            {
                return new int[0];
            }
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
            return Task<IEnumerable<ItemOrderStats>>.Factory.TryRun(
                () =>
                {
                    var cachedItems = new List<ItemOrderStats>();
                    var typesToRequest = new List<int>();
                    string cacheKey;
                    var dirtyCache = new Dictionary<int, ItemOrderStats>();
                    if (systemId.HasValue)
                    {
                        cacheKey = CalcCacheKey(new[] { systemId.Value });
                    }
                    else if (includeRegions != null && includeRegions.Any())
                    {
                        cacheKey = CalcCacheKey(includeRegions);
                    }
                    else
                    {
                        cacheKey = CalcCacheKey(new[] { JitaSystemId });
                    }

                    foreach (int typeId in typeIds.Distinct())
                    {
                        var itemStats = _regionDataCache.Get<ItemOrderStats>(ItemKeyFormat.FormatInvariant(typeId, cacheKey));
                        if (itemStats != null && itemStats.Data != null && !itemStats.IsDirty)
                        {
                            cachedItems.Add(itemStats.Data);
                        }
                        else
                        {
                            // queue for refresh
                            typesToRequest.Add(typeId);

                            // add to dirty cache incase of no data/error
                            if (itemStats != null && itemStats.IsDirty)
                            {
                                dirtyCache.Add(typeId, itemStats.Data);
                            }
                        }
                    }
                   
                    if (typesToRequest.Any())
                    {
                        // make the request for the types we don't have valid caches for
                        NameValueCollection requestParameters = this.CreateMarketRequestParameters(typesToRequest, includeRegions, systemId ?? 0, minQuantity);

                        Task<WebResponse> requestTask = WebRequestHelper.PostAsync(new Uri(EveCentralBaseUrl + MarketStatApi), requestParameters, _proxyServerAddress, _useDefaultCredential, _proxyUserName, _proxyPassword, _useBasicAuth);
                        requestTask.Wait(); // wait for the completion (we're in a background task anyways)

                        if (requestTask.IsCompleted && requestTask.Result !=null && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null)
                        {
                            using (Stream stream = requestTask.Result.GetResponseStream())
                            {
                                try
                                {
                                    // process result
                                    IEnumerable<ItemOrderStats> retrievedItems = GetOrderStatsFromXml(stream);

                                    // cache it.
                                    foreach (ItemOrderStats item in retrievedItems)
                                    {
                                        _regionDataCache.Add(ItemKeyFormat.FormatInvariant(item.ItemTypeId, cacheKey), item, DateTimeOffset.Now.Add(_cacheTtl));
                                        cachedItems.Add(item);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // todo: log/report XML parsing error.
                                    throw ex;
                                }
                            }
                        }
                        //TODO: log warning/error when task doesn't complete properly.

                        // add in "dirty/expired" items as filler for items that didn't get downloaded or to smooth over loss of connection issues
                        foreach (var itemKey in dirtyCache.Keys.Where(itemKey => cachedItems.All(c => c.ItemTypeId != itemKey)))
                        {
                            cachedItems.Add(dirtyCache[itemKey]);
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
            return Task<ItemMarketOrders>.Factory.TryRun(
                () =>
                {
                    string cacheKey;
                    if (systemId.HasValue)
                    {
                        cacheKey = CalcCacheKey(new[] { systemId.Value });
                    }
                    else if (includedRegions != null && includedRegions.Any())
                    {
                        cacheKey = CalcCacheKey(includedRegions);
                    }
                    else
                    {
                        cacheKey = CalcCacheKey(new[] { JitaSystemId });
                    }

                    var cachedData = _marketOrderCache.Get<ItemMarketOrders>(ItemKeyFormat.FormatInvariant(itemTypeId, cacheKey));
                    ItemMarketOrders itemData = null;
                    if (cachedData == null || cachedData.IsDirty)
                    {
                        // make the request for the types we don't have valid caches for
                        NameValueCollection requestParameters = this.CreateMarketRequestParameters(new[] { itemTypeId }, includedRegions, systemId ?? 0, minQuantity);
                        Task<WebResponse> requestTask = WebRequestHelper.PostAsync(new Uri(EveCentralBaseUrl + QuickLookApi), requestParameters, _proxyServerAddress, _useDefaultCredential, _proxyUserName, _proxyPassword, _useBasicAuth);
                        requestTask.Wait(); // wait for the completion (we're in a background task anyways)

                        if (requestTask.IsCompleted && !requestTask.IsCanceled && !requestTask.IsFaulted && requestTask.Exception == null && requestTask.Result != null)
                        {
                            using (Stream stream = requestTask.Result.GetResponseStream())
                            {
                                try
                                {
                                    // process result
                                    ItemMarketOrders data = this.GetMarketOrdersFromXml(stream);

                                    _marketOrderCache.Add(ItemKeyFormat.FormatInvariant(itemTypeId, cacheKey), data, DateTimeOffset.Now.Add(TimeSpan.FromHours(1)));


                                    itemData = data;
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

                    return itemData ?? (cachedData != null ? cachedData.Data : null);
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
            return WebRequestHelper.PostAsync(requestUri, paramData, _proxyServerAddress, _useDefaultCredential, _proxyUserName, _proxyPassword, _useBasicAuth).ContinueWith(
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
                    orderData.ItemTypeId = StringExtensions.ToInt(dataElement.Element("item").Value);
                    orderData.ItemName = dataElement.Element("itemname").Value;
                    orderData.Regions = new HashSet<int>(dataElement.Element("regions").Value.Split(',').Select(region => StringExtensions.ToInt(region)).Where(region => region > 0));
                    orderData.Hours = StringExtensions.ToInt(dataElement.Element("hours").Value);
                    orderData.MinQuantity = StringExtensions.ToInt(dataElement.Element("minqty").Value);

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
                   let orderId = StringExtensions.ToLong(order.Attribute("id").Value)
                   let regionId = StringExtensions.ToInt(order.Element("region").Value)
                   let stationId = StringExtensions.ToInt(order.Element("station").Value)
                   let stationName = order.Element("station_name").Value
                   let security = StringExtensions.ToDouble(order.Element("security").Value)
                   let range = StringExtensions.ToInt(order.Element("range").Value)
                   let price = StringExtensions.ToDouble(order.Element("price").Value)
                   let quantityRemaining = StringExtensions.ToInt(order.Element("vol_remain").Value)
                   let minQuantity = StringExtensions.ToInt(order.Element("min_volume").Value)
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
        private IEnumerable<ItemOrderStats> GetOrderStatsFromXml(Stream stream)
        {
            IEnumerable<ItemOrderStats> orderStats = null;
            using (TextReader reader = new StreamReader(stream))
            {
                // ReSharper disable PossibleNullReferenceException
                XDocument xml = XDocument.Load(reader);
                if (xml.Root != null && xml.Root.Element("marketstat") != null)
                {
                    orderStats = (from stats in xml.Root.Elements("marketstat")
                                  from type in stats.Elements("type")
                                  let typeId = StringExtensions.ToInt(type.Attribute("id").Value)
                                  let buyData = this.GetOrderData(type.Element("buy"))
                                  let sellData = this.GetOrderData(type.Element("sell"))
                                  let allData = this.GetOrderData(type.Element("all"))
                                  select new ItemOrderStats { ItemTypeId = typeId, Buy = buyData, Sell = sellData, All = allData });
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
            long volume = StringExtensions.ToLong(element.Element("volume").Value);

            double avg = StringExtensions.ToDouble(element.Element("avg").Value);
            double max = StringExtensions.ToDouble(element.Element("max").Value);
            double min = StringExtensions.ToDouble(element.Element("min").Value);
            double stddev = StringExtensions.ToDouble(element.Element("stddev").Value);
            double median = StringExtensions.ToDouble(element.Element("median").Value);
            double percentile = StringExtensions.ToDouble(element.Element("percentile").Value);

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