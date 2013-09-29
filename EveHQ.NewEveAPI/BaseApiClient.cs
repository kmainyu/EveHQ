﻿//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (BaseApiClient.cs), is part of EveHQ.
// 
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
// 
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================

namespace EveHQ.EveApi
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>
    /// Object used for interacting with the Eve API web service provided by CCP.
    /// </summary>
    public abstract class BaseApiClient : IDisposable
    {
        /// <summary>
        /// Default location of the EVE Online Web Service.
        /// </summary>
        public const string DefaultEveWebServiceLocation = "https://api.eveonline.com";

        /// <summary>
        /// name of results element.
        /// </summary>
        private const string Result = "result";

        /// <summary>
        /// File/Memory backed cache of data.
        /// </summary>
        private readonly ICacheProvider _cache;

        /// <summary>
        /// Location of the web service end point
        /// </summary>
        private readonly string _eveWebServiceLocation;

        /// <summary>
        /// request provider.
        /// </summary>
        private readonly IHttpRequestProvider _provider;

        /// <summary>
        /// Initializes a new instance of the BaseApiClient class.
        /// </summary>
        /// <param name="eveServiceLocation">Location of the web service end point</param>
        /// <param name="cache">location the cache will be stored in.</param>
        /// <param name="provider">request provider object.</param>
        protected internal BaseApiClient(string eveServiceLocation, ICacheProvider cache, IHttpRequestProvider provider)
        {
            _eveWebServiceLocation = eveServiceLocation;
            _cache = cache;
            _provider = provider;
        }

        /// <summary>
        /// Disposes the object and cleans up resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

         protected internal Task<EveServiceResponse<T>> GetServiceResponseAsync<T>(
             string keyId, string vCode, int characterId, string servicePath, IDictionary<string, string> callParams, string cacheKey, int defaultCacheSeconds, Func<XElement, T> xmlParseDelegate)
         {
             if (callParams == null)
             {
                 callParams = new Dictionary<string, string>();
             }

             if (!keyId.IsNullOrWhiteSpace())
             {
                 callParams.Add(ApiConstants.KeyId, keyId);
             }

             if (!vCode.IsNullOrWhiteSpace())
             {
                 callParams.Add(ApiConstants.VCode, vCode);
             }

             if (characterId > 0)
             {
                 callParams.Add(ApiConstants.CharacterId, characterId.ToInvariantString());
             }

             return this.GetServiceResponseAsync(servicePath, callParams, cacheKey, defaultCacheSeconds, xmlParseDelegate);
         }

        /// <summary>
        /// Initiates an async request to the service provider.
        /// </summary>
        /// <typeparam name="T">ApiType of the entity object to contain the response data.</typeparam>
        /// <param name="servicePath">relative url path to the service method.</param>
        /// <param name="callParams">A collection of name/value parameters to send on the request.</param>
        /// <param name="cacheKey">key used for caching data</param>
        /// <param name="defaultCacheSeconds">how long to cache data if service doesn't provide a value</param>
        /// <param name="xmlParseDelegate">the delegate for parsing the xml.</param>
        /// <returns>A reference to the async task.</returns>
        protected internal Task<EveServiceResponse<T>> GetServiceResponseAsync<T>(
            string servicePath, IDictionary<string, string> callParams, string cacheKey, int defaultCacheSeconds, Func<XElement, T> xmlParseDelegate)
        {
            Uri temp;
            if (!Uri.TryCreate(_eveWebServiceLocation + servicePath, UriKind.Absolute, out temp))
            {
                throw new InvalidOperationException("\"{0}\" and \"{1}\" cannot be combined to form a proper Url".FormatInvariant(_eveWebServiceLocation, servicePath));
            }

            // check cache for data and return if cached data exists and is still valid.
            CacheItem<EveServiceResponse<T>> resultData = GetCacheEntry<T>(cacheKey);

            Task<EveServiceResponse<T>> resultTask;

            if (resultData != null)
            {
                if (resultData.IsDirty)
                {
                    Task.Factory.TryRun(() => _provider.PostAsync(temp, callParams).ContinueWith(webTask => ProcessServiceResponse(webTask, cacheKey, defaultCacheSeconds, xmlParseDelegate)));
                }
                resultTask = ReturnCachedResponse(resultData);
            }
            else
            {
                resultTask = _provider.PostAsync(temp, callParams).ContinueWith(webTask => ProcessServiceResponse(webTask, cacheKey, defaultCacheSeconds, xmlParseDelegate));
            }

            return resultTask;

        }

        /// <summary>
        /// Disposes the object and cleans up resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        /// Gets the data from caching system if it exists.
        /// </summary>
        /// <typeparam name="T">ApiType of data.</typeparam>
        /// <param name="key">The key to retrieve</param>
        /// <returns>And instance of the service response object.</returns>
        protected CacheItem<EveServiceResponse<T>> GetCacheEntry<T>(string key)
        {
            return _cache.Get<EveServiceResponse<T>>(key);
        }

        /// <summary>
        /// Gets the caching details from the service response if exists, or uses the provided defaults.
        /// </summary>
        /// <param name="root">root of the xml response</param>
        /// <param name="defaultSeconds">default value to use if no data is found.</param>
        /// <returns>returns the time stamp of when the data expires.</returns>
        private static DateTimeOffset GetCacheExpiryFromResponse(XElement root, int defaultSeconds)
        {
            DateTime temp;
            XElement xElement = root.Element("cachedUntil");
            if (xElement != null && DateTime.TryParse(xElement.Value, out temp))
            {
                // web service is in UTC despite not advertising an offset.
                return new DateTimeOffset(temp, TimeSpan.FromSeconds(0));
            }

            return DateTimeOffset.Now.AddSeconds(defaultSeconds);
        }

        /// <summary>
        /// Gets the content of an HTTP Response and reads the content as XML.
        /// </summary>
        /// <param name="message">The service response message</param>
        /// <returns>Document XML</returns>
        private static XDocument GetXmlFromResponse(HttpResponseMessage message)
        {
            XDocument content = null;

            if (message != null && message.Content != null)
            {
                // read the content as a string.
                Task<string> readTask = message.Content.ReadAsStringAsync();
                readTask.Wait();

                // parse the string as XML. The caller needs to handle the parsing errors.
                content = XDocument.Parse(readTask.Result);
            }

            return content;
        }

        /// <summary>
        /// Creates a TPL Task for passing cached responses back to callers.
        /// </summary>
        /// <typeparam name="T">ApiType of the data to be return</typeparam>
        /// <param name="cachedResult">cached data.</param>
        /// <returns>A TPL task.</returns>
        private static Task<EveServiceResponse<T>> ReturnCachedResponse<T>(CacheItem<EveServiceResponse<T>> cachedResult)
        {
            // cachedResult was found in cache.
            cachedResult.Data.CachedResponse = true;

            // return the task with the response value in it.
            return Task.Factory.StartNew(() => cachedResult.Data);
        }

        /// <summary>
        /// Processes the response from the remote service
        /// </summary>
        /// <typeparam name="T">Data entity to use as the response type</typeparam>
        /// <param name="webTask">task that was used to call the web service.</param>
        /// <param name="cacheKey">cache key for caching the response.</param>
        /// <param name="defaultCacheSeconds">default cache time used if the service doesn't provide it.</param>
        /// <param name="parseXml">delegate for parsing the xml into the strong data type.</param>
        /// <returns>The strongly typed data response.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Required in order to pass exception information to callers.")]
        private EveServiceResponse<T> ProcessServiceResponse<T>(Task<HttpResponseMessage> webTask, string cacheKey, int defaultCacheSeconds, Func<XElement, T> parseXml)
        {
            Exception faultError = null;
            T result = default(T);
            DateTimeOffset cacheTime = DateTimeOffset.Now.AddSeconds(defaultCacheSeconds);
            if (webTask.IsFaulted)
            {
                faultError = webTask.Exception;
            }

            try
            {
                // Get the xml from the response.
                XDocument xml = GetXmlFromResponse(webTask.Result);
                XElement resultsElement;
                if (xml != null && xml.Root != null && (resultsElement = xml.Root.Element(Result)) != null)
                {
                    // using LINQ convert the XML into the collection of characters.
                    result = parseXml(resultsElement);
                    cacheTime = GetCacheExpiryFromResponse(xml.Root, defaultCacheSeconds);
                }
                // TODO: handle when there is no data, or when the service returns a server side error (auth error or no access for instance).
            }
            catch (Exception e)
            {
                // catch any of the xml processing errors
                faultError = e;
            }

            // store it.
            var eveResult = new EveServiceResponse<T>(result, faultError, webTask.Result.StatusCode, cacheTime);

            // cache it
            SetCacheEntry(cacheKey, eveResult);

            return eveResult;
        }

        /// <summary>
        /// Sets data into cache.
        /// </summary>
        /// <typeparam name="T">type of data being stored.</typeparam>
        /// <param name="key">key to store the data under.</param>
        /// <param name="data">date to store.</param>
        private void SetCacheEntry<T>(string key, EveServiceResponse<T> data)
        {
            _cache.Add(key, data, data.CacheUntil);
        }
    }
}