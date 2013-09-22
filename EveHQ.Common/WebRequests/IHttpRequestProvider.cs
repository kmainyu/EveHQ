// ===========================================================================
// <copyright file="IHttpRequestProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (IHttpRequestProvider.cs), is part of EveHQ.
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
    using System.Collections.Specialized;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>The HttpRequestProvider interface.</summary>
    public interface IHttpRequestProvider
    {
        #region Public Methods and Operators

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth, string acceptContentType);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <param name="completionOption">The completion Option.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(
            Uri target, 
            Uri proxyServerAddress, 
            bool useDefaultCredential, 
            string proxyUserName, 
            string proxyPassword, 
            bool useBasicAuth, 
            string acceptContentType, 
            HttpCompletionOption completionOption);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, string postContent);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, string postContent, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <param name="proxyServerAddress">The proxy Server Address.</param>
        /// <param name="useDefaultCredential">The use Default Credential.</param>
        /// <param name="proxyUserName">The proxy User Name.</param>
        /// <param name="proxyPassword">The proxy Password.</param>
        /// <param name="useBasicAuth">The use Basic Auth.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, NameValueCollection postData, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);

        #endregion
    }
}