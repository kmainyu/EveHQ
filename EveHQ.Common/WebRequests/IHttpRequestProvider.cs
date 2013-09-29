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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>The HttpRequestProvider interface.</summary>
    public interface IHttpRequestProvider
    {
        #region Public Methods and Operators

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, string acceptContentType);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="acceptContentType">The accept Content Type.</param>
        /// <param name="completionOption">The completion Option.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, string acceptContentType, HttpCompletionOption completionOption);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> PostAsync(Uri target, string postContent);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> PostAsync(Uri target, NameValueCollection postData);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> PostAsync(Uri target, IDictionary<string, string> postData);

        #endregion
    }
}