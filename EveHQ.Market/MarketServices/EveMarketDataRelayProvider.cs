// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (EveMarketDataRelayProvider.cs), is part of EveHQ.
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
    using System.Net;
    using System.Threading.Tasks;

    using EveHQ.Common;
    using EveHQ.Market.UnifiedMarketDataFormat;

    /// <summary>
    /// Eve Market Data Relay uploader.
    /// </summary>
    public class EveMarketDataRelayProvider : IMarketDataReceiver
    {
        /// <summary>
        /// Where we will upload data to.
        /// </summary>
        private const string EmdrUploadUrl = "http://upload.eve-emdr.com/upload/";

        /// <summary>
        /// upload key.
        /// </summary>
        private readonly UploadKey _uploadKey = new UploadKey { Name = "EVE Market Data Relay", Key = "0" };

        /// <summary>
        /// is this provider enabled;
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        /// Next attempt at uploading if not currently enabled.
        /// </summary>
        private DateTimeOffset _nextAttempt = DateTimeOffset.Now;

        private IHttpRequestProvider _requestProvider;

        public EveMarketDataRelayProvider(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        /// <summary>Gets a value indicating whether this uploader is enabled.</summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
        }

        /// <summary>Gets the next attempt timestap.</summary>
        public DateTimeOffset NextAttempt
        {
            get
            {
                return _nextAttempt;
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

        /// <summary>Uploads the market data.</summary>
        /// <param name="marketDataJson">The market data JSON.</param>
        /// <returns>A reference to the Async Task.</returns>
        public Task UploadMarketData(string marketDataJson)
        {
            // creat the URL for the request
            var requestUri = new Uri(EmdrUploadUrl);

            // send the request and return the task handle after checking the return of the web request
            return _requestProvider.PostAsync(requestUri, marketDataJson).ContinueWith(
                task =>
                {
                    HttpWebResponse httpResponse;
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted && task.Exception == null && task.Result != null && (httpResponse = task.Result as HttpWebResponse) != null
                        && httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // success
                        _isEnabled = true;
                        _nextAttempt = DateTimeOffset.Now;
                    }
                    else
                    {
                        // there was something wrong... disable this receiver for a while.
                        _isEnabled = false;
                        _nextAttempt = DateTimeOffset.Now.AddHours(1);
                    }

                    return task;
                });
        }
    }
}