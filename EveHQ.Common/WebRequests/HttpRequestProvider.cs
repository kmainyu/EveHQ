// ===========================================================================
// <copyright file="HttpRequestProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (HttpRequestProvider.cs), is part of EveHQ.
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
namespace EveHQ.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using EveHQ.Common.Extensions;

    /// <summary>
    ///     Provider class for making Http requests.
    /// </summary>
    public sealed class HttpRequestProvider : IHttpRequestProvider
    {
        #region Static Fields

        /// <summary>
        ///     user agent value to send along on requests for provider collection.
        /// </summary>
        //  private static readonly string UserAgent = "EveHQ v" + Assembly.GetExecutingAssembly().GetName().Version;

        #endregion

        #region Fields

        /// <summary>The _proxy info.</summary>
        private readonly WebProxyDetails _proxyInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="HttpRequestProvider"/> class.</summary>
        /// <param name="proxyInfo">The proxy info.</param>
        public HttpRequestProvider(WebProxyDetails proxyInfo)
        {
            _proxyInfo = proxyInfo;
        }

        #endregion

        #region Public Methods and Operators

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handler is used by the async task, and cannot be disposed early.")]
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
                    request.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptContentType));
                }

                return request.GetAsync(target, completionOption);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => null);
        }

        /// <summary>The post async.</summary>
        /// <param name="target">The target.</param>
        /// <param name="postContent">The post content.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<HttpResponseMessage> PostAsync(Uri target, string postContent)
        {
            return PostAsync(target, postContent, "text/plain");
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <param name="contentType">The content Type.</param>
        /// <returns>The asynchronouse task instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing the handler for async operations would cause the operation to fail."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "validated by extension method.")]
        public Task<HttpResponseMessage> PostAsync(Uri target, string postContent, string contentType)
        {
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

                HttpContent content = !postContent.IsNullOrWhiteSpace() ? new StringContent(postContent, Encoding.UTF8, contentType) : null;

                return requestClient.PostAsync(target, content);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => null);
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
        /// <returns>The <see cref="Task"/>.</returns>
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

        #endregion
    }
}