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
    /// Helper class for making Async Web requests with .net 3.5
    /// </summary>
    public static class WebRequestHelper
    {
        /// <summary>
        /// user agent value to send along on requests for provider collection.
        /// </summary>
        private static readonly string userAgent = "EveHQ v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        public static Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth,string acceptContentType=null, HttpCompletionOption completionOption= HttpCompletionOption.ResponseContentRead)
        {
            var handler = new HttpClientHandler();

            //var request = WebRequest.Create(target) as HttpWebRequest;
           // request.UserAgent = userAgent;
            // ReSharper disable PossibleNullReferenceException
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
            //request.UserAgent = userAgent;

            return request.GetAsync(target.ToString(),completionOption);


            // ReSharper restore PossibleNullReferenceException
        }


        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        public static Task<WebResponse> PostAsync(Uri target, string postContent)
        {
            return PostAsync(target, postContent, null, false, null, null, false);
        }

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        public static Task<WebResponse> PostAsync(Uri target, string postContent, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
        {
            var request = WebRequest.Create(target) as HttpWebRequest;
            // This is never null
            // ReSharper disable PossibleNullReferenceException
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
            request.UserAgent = userAgent;

            // ReSharper restore PossibleNullReferenceException
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = postContent.Length;
            Stream reqStream = request.GetRequestStream();

            byte[] dataBytes = Encoding.UTF8.GetBytes(postContent);

            reqStream.Write(dataBytes, 0, dataBytes.Length);
            reqStream.Flush();
            reqStream.Close();

            return Task<WebResponse>.Factory.FromAsync(
                request.BeginGetResponse,
                (ticket) =>
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

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <returns>The asynchronouse task instance</returns>
        public static Task<WebResponse> PostAsync(Uri target, NameValueCollection postData, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth)
        {
            var data = new List<string>();
            foreach (string key in postData.AllKeys)
            {
                data.AddRange(postData[key].Split(',').Select(value => key + "=" + value).ToArray());
            }

            string paramData = string.Join("&", data.ToArray());

            return PostAsync(target, paramData, proxyServerAddress, useDefaultCredential, proxyUserName, proxyPassword, useBasicAuth);
        }
    }
}