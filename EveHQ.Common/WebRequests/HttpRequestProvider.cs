// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (WebRequestHelper.cs), is part of EveHQ.
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
namespace EveHQ.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using EveHQ.Common.Extensions;

    /// <summary>
    /// Provider class for making Http requests.
    /// </summary>
    public class HttpRequestProvider : IHttpRequestProvider
    {
        private static readonly HttpRequestProvider DefaultProviderInstance = new HttpRequestProvider(); // type initializer will make this on first use.

        /// <summary>
        /// user agent value to send along on requests for provider collection.
        /// </summary>
        private static readonly string UserAgent = "EveHQ v" + Assembly.GetExecutingAssembly().GetName().Version;

        private HttpRequestProvider()
        {
            // prevents instance creation
        }

        /// <summary>
        /// Gets the default http request provider.
        /// </summary>
        public static HttpRequestProvider Default
        {
            get
            {
                return DefaultProviderInstance;
            }
        }


        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress"></param>
        /// <param name="useDefaultCredential"></param>
        /// <param name="proxyUserName"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useBasicAuth"></param>        
        /// <returns>The asynchronouse task instance</returns>
        public Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
        {
            return GetAsync(target, proxyServerAddress, useDefaultCredential, proxyUserName, proxyPassword, useBasicAuth, null, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress"></param>
        /// <param name="useDefaultCredential"></param>
        /// <param name="proxyUserName"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useBasicAuth"></param>        
        /// <returns>The asynchronouse task instance</returns>
        public Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth, string acceptContentType)
        {
            return GetAsync(target, proxyServerAddress, useDefaultCredential, proxyUserName, proxyPassword, useBasicAuth, acceptContentType, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress"></param>
        /// <param name="useDefaultCredential"></param>
        /// <param name="proxyUserName"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useBasicAuth"></param>
        /// <param name="acceptContentType"></param>
        /// <param name="completionOption"></param>
        /// <returns>The asynchronouse task instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handler is used by the async task, and cannot be disposed early.")]
        public Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth, string acceptContentType, HttpCompletionOption completionOption)
        {

            if (target != null)
            {

                var handler = new HttpClientHandler();

                if (proxyServerAddress != null)
                {
                    // set proxy if required.
                    var proxy = new WebProxy(proxyServerAddress);
                    if (useDefaultCredential)
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    else
                    {
                        var credential = new NetworkCredential(proxyUserName, proxyPassword);
                        proxy.Credentials = useBasicAuth ? credential.GetCredential(proxyServerAddress, "Basic") : credential;
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


        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        public Task<WebResponse> PostAsync(Uri target, string postContent)
        {
            return PostAsync(target, postContent, null, false, null, null, false);
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <param name="proxyServerAddress"></param>
        /// <param name="useDefaultCredential"></param>
        /// <param name="proxyUserName"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useBasicAuth"></param>
        /// <returns>The asynchronouse task instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification="validated by extension method."), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catching for logging purposes.")]
        public Task<WebResponse> PostAsync(Uri target, string postContent, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
        {
            if (target != null && !postContent.IsNullOrWhiteSpace())
            {
                // TODO: Update this to use HTTP Client.
                var request = WebRequest.Create(target) as HttpWebRequest;

                // This is never null
                if (proxyServerAddress != null)
                {
                    // set proxy if required.
                    var proxy = new WebProxy(proxyServerAddress);
                    if (useDefaultCredential)
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    else
                    {
                        var credential = new NetworkCredential(proxyUserName, proxyPassword);
                        proxy.Credentials = useBasicAuth ? credential.GetCredential(proxyServerAddress, "Basic") : credential;
                    }

                    request.Proxy = proxy;
                }


                request.Method = "POST";
                request.UserAgent = UserAgent;
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = postContent.Length;
                Stream reqStream = request.GetRequestStream();

                byte[] dataBytes = Encoding.UTF8.GetBytes(postContent);

                reqStream.Write(dataBytes, 0, dataBytes.Length);
                reqStream.Flush();
                reqStream.Close();

                return Task<WebResponse>.Factory.FromAsync(
                    request.BeginGetResponse,
                    ticket =>
                    {
                        WebResponse response = null;
                        try
                        {
                            response = request.EndGetResponse(ticket);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError("Error with web request to {0} : {1}".FormatInvariant(target, ex.Message));
                        }

                        return response;
                    },
                    null);
            }
            return Task<WebResponse>.Factory.StartNew(() => null);
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <param name="proxyServerAddress"></param>
        /// <param name="useDefaultCredential"></param>
        /// <param name="proxyUserName"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="useBasicAuth"></param>
        /// <returns>The asynchronouse task instance</returns>
        public Task<WebResponse> PostAsync(Uri target, NameValueCollection postData, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
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

            return PostAsync(target, paramData, proxyServerAddress, useDefaultCredential, proxyUserName, proxyPassword, useBasicAuth);
        }
    }
}