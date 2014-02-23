// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System;
using System.Net;
using System.Threading.Tasks;
using EveHQ.Common;
using EveHQ.Market.UnifiedMarketDataFormat;

namespace EveHQ.Market
{
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

        /// <summary>The _request provider.</summary>
        private readonly IHttpRequestProvider _requestProvider;

        /// <summary>
        ///     upload key.
        /// </summary>
        private readonly UploadKey _uploadKey = new UploadKey {Name = "EVE Market Data Relay", Key = "0"};

        /// <summary>
        ///     is this provider enabled;
        /// </summary>
        private bool _isEnabled = true;

        /// <summary>
        ///     Next attempt at uploading if not currently enabled.
        /// </summary>
        private DateTimeOffset _nextAttempt = DateTimeOffset.Now;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="EveMarketDataRelayProvider" /> class.</summary>
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
            get { return _isEnabled; }
        }

        /// <summary>Gets the next attempt timestap.</summary>
        public DateTimeOffset NextAttempt
        {
            get { return _nextAttempt; }
        }

        /// <summary>Gets the unified upload key.</summary>
        public UploadKey UnifiedUploadKey
        {
            get { return _uploadKey; }
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
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted && task.Exception == null &&
                        task.Result != null && task.Result.StatusCode == HttpStatusCode.OK)
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