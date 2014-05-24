//  ==============================================================================
//  
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2014  EveHQ Development Team
//    
//  This file is part of EveHQ.
//   
//  The source code for EveHQ is free and you may redistribute 
//  it and/or modify it under the terms of the MIT License. 
//  
//  Refer to the NOTICES file in the root folder of EVEHQ source
//  project for details of 3rd party components that are covered
//  under their own, separate licenses.
//  
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
//  license below for details.
//  
//  ------------------------------------------------------------------------------
//  
//  The MIT License (MIT)
//  
//  Copyright © 2005-2014  EveHQ Development Team
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  
// ==============================================================================

namespace EveHQ.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using EveHQ.Common.Extensions;

    /// <summary>
    ///     Provider class for making Http requests.
    /// </summary>
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        //  private static readonly string UserAgent = "EveHQ v" + Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        ///     user agent value to send along on requests for provider collection.
        /// </summary>
        /// <summary>The _proxy info.</summary>
        private readonly WebProxyDetails _proxyInfo;

        /// <summary>Initializes a new instance of the <see cref="HttpRequestProvider" /> class.</summary>
        /// <param name="proxyInfo">The proxy info.</param>
        public HttpRequestProvider(WebProxyDetails proxyInfo)
        {
            _proxyInfo = proxyInfo;
        }

        private string _version;
        private string Version
        {
            get
            {
                if (_version.IsNullOrWhiteSpace())
                {
                    _version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                }
                return _version;
            }
        }

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        public Task<HttpResponseMessage> GetAsync(Uri target)
        {
            return GetAsync(target, null, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <returns>The asynchronouse task instance</returns>
        public Task<HttpResponseMessage> GetAsync(Uri target, string acceptContentType)
        {
            return GetAsync(target, acceptContentType, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <param name="completionOption">The completion Option.</param>
        /// <returns>The asynchronouse task instance</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handler is used by the async task, and cannot be disposed early.")]
        public Task<HttpResponseMessage> GetAsync(Uri target, string acceptContentType, HttpCompletionOption completionOption)
        {
            if (target != null)
            {
                var handler = new HttpClientHandler();

                if (_proxyInfo != null && _proxyInfo.ProxyServerAddress != null)
                {
                    // set proxy if required.
                    var proxy = new WebProxy(_proxyInfo.ProxyServerAddress);
                    if (_proxyInfo.UseDefaultCredential)
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    else
                    {
                        var credential = new NetworkCredential(_proxyInfo.ProxyUserName, _proxyInfo.ProxyPassword);
                        proxy.Credentials = _proxyInfo.UseBasicAuth ? credential.GetCredential(_proxyInfo.ProxyServerAddress, "Basic") : credential;
                    }

                    handler.Proxy = proxy;
                    handler.UseProxy = true;
                }

                handler.AutomaticDecompression = DecompressionMethods.GZip;
                handler.AllowAutoRedirect = true;

                var request = new HttpClient(handler);

                if (!acceptContentType.IsNullOrWhiteSpace())
                {
                    request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptContentType));
                }

                return request.GetAsync(target, completionOption);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => null);
        }

        /// <summary>The post async.</summary>
        /// <param name="target">The target.</param>
        /// <param name="postContent">The post content.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<HttpResponseMessage> PostAsync(Uri target, string postContent)
        {
            return PostAsync(target, postContent, "text/plain");
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <returns>The asynchronouse task instance</returns>
        public Task<HttpResponseMessage> PostAsync(Uri target, NameValueCollection postData)
        {
            var data = new List<string>();

            if (postData != null)
            {
                foreach (string key in postData.AllKeys)
                {
                    data.AddRange(postData[key].Split(',').Select(value => key + "=" + value).ToArray());
                }
            }

            string paramData = string.Join("&", data.ToArray());

            return PostAsync(target, paramData, "application/x-www-form-urlencoded");
        }

        /// <summary>The post async.</summary>
        /// <param name="target">The target.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<HttpResponseMessage> PostAsync(Uri target, IDictionary<string, string> postData)
        {
            var data = new List<string>();

            if (postData != null)
            {
                foreach (string key in postData.Keys)
                {
                    data.AddRange(postData[key].Split(',').Select(value => key + "=" + value).ToArray());
                }
            }

            string paramData = string.Join("&", data.ToArray());

            return PostAsync(target, paramData, "application/x-www-form-urlencoded");
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <param name="contentType">The content Type.</param>
        /// <returns>The asynchronouse task instance</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing the handler for async operations would cause the operation to fail.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "validated by extension method.")]
        public Task<HttpResponseMessage> PostAsync(Uri target, string postContent, string contentType)
        {

            const string UserAgent = "user-agent";
            const string UserAgentValue = "EveHQ v{0}";
            if (target != null)
            {
                var handler = new HttpClientHandler();

                // This is never null
                if (_proxyInfo != null && _proxyInfo.ProxyServerAddress != null)
                {
                    // set proxy if required.
                    var proxy = new WebProxy(_proxyInfo.ProxyServerAddress);
                    if (_proxyInfo.UseDefaultCredential)
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    else
                    {
                        var credential = new NetworkCredential(_proxyInfo.ProxyUserName, _proxyInfo.ProxyPassword);
                        proxy.Credentials = _proxyInfo.UseBasicAuth ? credential.GetCredential(_proxyInfo.ProxyServerAddress, "Basic") : credential;
                    }

                    handler.Proxy = proxy;
                    handler.UseProxy = true;
                }

                handler.AutomaticDecompression = DecompressionMethods.GZip;
                handler.AllowAutoRedirect = true;

                var requestClient = new HttpClient(handler);
                requestClient.DefaultRequestHeaders.Add(UserAgent, UserAgentValue.FormatInvariant(Version));

                HttpContent content = !postContent.IsNullOrWhiteSpace() ? new StringContent(postContent, Encoding.UTF8, contentType) : null;

                return requestClient.PostAsync(target, content);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => null);
        }
    }
}