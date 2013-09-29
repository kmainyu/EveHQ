// ===========================================================================
// <copyright file="EveMarketDataRelayProvider.cs" company="EveHQ Development Team">
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
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.Market
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using EveHQ.Common;
    using EveHQ.Market.UnifiedMarketDataFormat;

    /// <summary>
    ///     Eve Market Data Relay uploader.
    /// </summary>
    public class EveMarketDataRelayProvider : IMarketDataReceiver
    {
        #region Constants

        /// <summary>
        ///     Where we will upload data to.
        /// </summary>
        private const string EmdrUploadUrl = "http://upload.eve-emdr.com/upload/";

        #endregion

        #region Fields

        /// <summary>
        ///     upload key.
        /// </summary>
        private readonly UploadKey _uploadKey = new UploadKey { Name = "EVE Market Data Relay", Key = "0" };

        /// <summary>
        ///     is this provider enabled;
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        ///     Next attempt at uploading if not currently enabled.
        /// </summary>
        private DateTimeOffset _nextAttempt = DateTimeOffset.Now;

        /// <summary>The _request provider.</summary>
        private IHttpRequestProvider _requestProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="EveMarketDataRelayProvider"/> class.</summary>
        /// <param name="requestProvider">The request provider.</param>
        public EveMarketDataRelayProvider(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods and Operators

        /// <summary>Uploads the market data.</summary>
        /// <param name="marketData">The market data JSON.</param>
        /// <returns>A reference to the Async Task.</returns>
        public Task UploadMarketData(string marketData)
        {
            // creat the URL for the request
            var requestUri = new Uri(EmdrUploadUrl);

            // send the request and return the task handle after checking the return of the web request
            return _requestProvider.PostAsync(requestUri, marketData).ContinueWith(
                task =>
                    {
                        HttpWebResponse httpResponse;
                        if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted && task.Exception == null && task.Result != null && task.Result.StatusCode == HttpStatusCode.OK)
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

        #endregion
    }
}