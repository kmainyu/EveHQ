// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (MarketUploader.cs), is part of EveHQ.
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EveCacheParser;

    using EveHQ.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MarketUploader
    {
        /// <summary>The iso 8601 format.</summary>
        private const string Iso8601Format = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary>The _running.</summary>
        private static bool _running;

        /// <summary>The _stopping.</summary>
        private static bool _stopping;

        /// <summary>The _custom eve app data path.</summary>
        private readonly string _customEveAppDataPath;

        /// <summary>The _market receivers.</summary>
        private readonly IEnumerable<IMarketDataReceiver> _marketReceivers;

        /// <summary>The _last file change time.</summary>
        private DateTimeOffset _lastFileChangeTime;

        /// <summary>Initializes a new instance of the <see cref="MarketUploader"/> class.</summary>
        /// <param name="lastFileChangeTime">The last file change time.</param>
        /// <param name="eveAppDataPath">The eve app data path.</param>
        public MarketUploader(DateTimeOffset lastFileChangeTime, IEnumerable<IMarketDataReceiver> marketReceivers, string eveAppDataPath)
        {
            _lastFileChangeTime = lastFileChangeTime;
            _customEveAppDataPath = eveAppDataPath;

            _marketReceivers = marketReceivers;
        }

        /// <summary>
        /// Gets the timestamp of the most recently updated cache file.
        /// </summary>
        public DateTimeOffset LastCacheFileChangeTime
        {
            get
            {
                return _lastFileChangeTime;
            }
        }

        /// <summary>The start.</summary>
        public void Start()
        {
            if (_running)
            {
                return;
            }

            _running = true;
            Task.Factory.TryRun(this.StartUploader);
        }

        /// <summary>The stop.</summary>
        public void Stop()
        {
            _stopping = true;
        }

        /// <summary>The start uploader.</summary>
        private void StartUploader()
        {
            DateTimeOffset nextCycle = DateTimeOffset.Now.AddSeconds(20);
            while (_running)
            {
                if (nextCycle > DateTimeOffset.Now)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                UploadMarketData();
                nextCycle = DateTimeOffset.Now.AddSeconds(20);
            }

            _stopping = false;
        }

        /// <summary>The upload market data.</summary>
        private void UploadMarketData()
        {
            // check to see if there are any changed files (based on write time)
            IEnumerable<FileInfo> changedFiles = Parser.GetMachoNetCachedFiles(_customEveAppDataPath).Where(fi => fi.LastWriteTimeUtc > _lastFileChangeTime);

            if (!changedFiles.Any())
            {
                // no changed files this run
                return;
            }

            // there are changed files, so parse them for data and format it into unified data format.
            IEnumerable<string> changedData = changedFiles.Select(
                file =>
                {
                    var result = new KeyValuePair<object, object>();
                    try
                    {
                        result = Parser.Parse(file);
                    }
                    catch (ParserException pex)
                    {
                        Trace.TraceWarning("A Parser Exception occured on file {0}. The error was : {1}".FormatInvariant(file.Name, pex.Message));
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("An unexpected error occured processing {0}. The error was :{1}\r\nStack Trace:{2}".FormatInvariant(file.Name, ex.Message, ex.StackTrace));
                    }

                    return result;
                }).Select(data => NormalizeFormat(data, _marketReceivers)).Where(data => data != null).Select(data => data.ToJson());

            foreach (string jsonData in changedData)
            {
                // upload each json data item to the market receivers.
                string marketData = jsonData; // avoid foreach access in linq closure
                Task.WaitAll(
                    _marketReceivers.Where(receiver => receiver.IsEnabled || (!receiver.IsEnabled && receiver.NextAttempt < DateTimeOffset.Now))
                                    .Select(uploadService => uploadService.UploadMarketData(marketData))
                                    .ToArray());
            }

            // set the last change time to the most recent file in the set.
            _lastFileChangeTime = changedFiles.Select(file => file.LastWriteTimeUtc).Max();
        }

        /// <summary>Formats the weakly typed data scraped from the cache file into a C# representation of the Unified Upload Data Format. See http://dev.eve-central.com/unifieduploader/start for details. </summary>
        /// <param name="result">The result Key/Value pair.</param>
        /// <param name="receivers">The collection of market receivers we will send the data to.</param>
        /// <returns>The strong typed data object.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "the spelling is purposeful.")]
        private static UnifiedDataFormat NormalizeFormat(KeyValuePair<object, object> result, IEnumerable<IMarketDataReceiver> receivers)
        {
            if (result.Key == null)
            {
                return null;
            }

            UnifiedDataFormat data;

            // the key of the KVP is actually a set of data types
            var objectTuple = result.Key as Tuple<object>;
            if (objectTuple == null)
            {
                return null;
            }

            var hashedObjects = objectTuple.Item1 as List<object>;
            if (hashedObjects == null || hashedObjects.Count < 4)
            {
                return null;
            }

            // convert it to an array so that indexer access is possible.
            object[] dataKeys = hashedObjects.ToArray();

            // check that this result contains market information
            if (dataKeys[0].ToString() != "marketProxy")
            {
                return null;
            }

            // these offsets are assumed to be fixed
            long regionId = dataKeys[2].ToLong();
            long typeId = dataKeys[3].ToLong();

            // the value half of the KVP is a dictionary of various data types.
            var valueBag = result.Value as Dictionary<object, object>;
            if (valueBag == null)
            {
                return null;
            }

            // Columns and Rows info
            var values = valueBag["lret"] as List<object>;
            if (values == null)
            {
                return null;
            }

            // time stamp collection... one is a windows 64bit timestamp.. the other, no idea.
            var versionInfo = valueBag["version"] as List<object>;
            if (versionInfo == null)
            {
                return null;
            }

            DateTimeOffset fileGenerationTime = DateTimeOffset.FromFileTime(versionInfo[0].ToLong()).ToUniversalTime();

            // create the UDF appropriate object based on the method name used for the cache file.
            switch (dataKeys[1].ToString())
            {
                // GetXXXPriceHistory gets the historical data for a given item.
                case "GetNewPriceHistory":
                case "GetOldPriceHistory":

                    var historyData = new UnifiedDataFormat<HistoryRowset> { Columns = HistoryRowset.ColumnNames };
                    var historyRows = new HistoryRowset { GeneratedAt = fileGenerationTime.ToString(Iso8601Format), RegionID = regionId.ToInvariantString(), TypeID = typeId.ToInvariantString() };
                    historyRows.Rows.AddRange(
                        values.Cast<Dictionary<object, object>>()
                              .Select(
                                  record =>
                                  new HistoryRow
                                  {
                                      Average = record["avgPrice"].ToDouble(),
                                      Date = DateTimeOffset.FromFileTime(record["historyDate"].ToLong()).ToUniversalTime().ToString(Iso8601Format),
                                      High = record["highPrice"].ToDouble(),
                                      Low = record["lowPrice"].ToDouble(),
                                      Orders = record["orders"].ToLong(),
                                      Quantity = record["volume"].ToLong(),
                                  }));
                    historyData.Rowsets.Add(historyRows);
                    historyData.ResultType = ResultKind.History;

                    data = historyData;
                    break;

                    // GetOrders is for the current active order list for an item.
                case "GetOrders":
                    var orderData = new UnifiedDataFormat<OrderRowset> { Columns = OrderRowset.ColumnNames };
                    var orderRowset = new OrderRowset { GeneratedAt = fileGenerationTime.ToString(Iso8601Format), RegionID = regionId.ToInvariantString(), TypeID = typeId.ToInvariantString() };
                    orderRowset.Rows.AddRange(
                        values.Cast<List<object>>()
                              .SelectMany(
                                  obj => obj.Cast<Dictionary<object, object>>(), 
                                  (obj, order) =>
                                  new OrderRow
                                  {
                                      Bid = order["bid"].ToBoolean(), 
                                      Duration = order["duration"].ToLong(), 
                                      IssueDate = DateTimeOffset.FromFileTime(order["issueDate"].ToLong()).ToUniversalTime().ToString(Iso8601Format), 
                                      MinVolume = order["minVolume"].ToLong(), 
                                      OrderId = order["orderID"].ToLong(), 
                                      Price = order["price"].ToDouble(), 
                                      Range = order["range"].ToLong(), 
                                      SolarSystemId = order["solarSystemID"].ToLong(), 
                                      StationId = order["stationID"].ToLong(), 
                                      VolEntered = order["volEntered"].ToLong(), 
                                      VolRemaining = order["volRemaining"].ToLong()
                                  }));
                    orderData.Rowsets.Add(orderRowset);
                    orderData.ResultType = ResultKind.Orders;

                    data = orderData;
                    break;

                    // any other value we encounter means this isn't a file we should be processing.
                default:
                    return null;
            }

            // add in the upload keys
            data.UploadKeys = new List<UploadKey>();
            data.UploadKeys.AddRange(receivers.Select(target => target.UnifiedUploadKey));

            return data;
        }
    }
}