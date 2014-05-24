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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using EveHQ.Caching;
using EveHQ.Common;
using EveHQ.Common.Extensions;

namespace EveHQ.EveApi
{
    using System.Diagnostics;
    using System.Net;

    /// <summary>
    ///     Object used for interacting with the Eve API web service provided by CCP.
    /// </summary>
    public abstract class BaseApiClient : IDisposable
    {
        /// <summary>
        ///     Default location of the EVE Online Web Service.
        /// </summary>
        public const string DefaultEveWebServiceLocation = "https://api.eveonline.com";

        /// <summary>
        ///     name of results element.
        /// </summary>
        private const string Result = "result";

        private const string Error = "error";

        /// <summary>
        ///     File/Memory backed cache of data.
        /// </summary>
        private readonly ICacheProvider _cache;

        /// <summary>
        ///     Location of the web service end point
        /// </summary>
        private readonly string _eveWebServiceLocation;

        /// <summary>
        ///     request provider.
        /// </summary>
        private readonly IHttpRequestProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the BaseApiClient class.
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
        ///     Disposes the object and cleans up resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected internal Task<EveServiceResponse<T>> GetServiceResponseAsync<T>(string keyId, string vCode,
            int characterId, string servicePath, IDictionary<string, string> callParams, string cacheKey,
            int defaultCacheSeconds, ResponseMode responseMode, Func<XElement, T> xmlParseDelegate)
            where T : class
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

            return GetServiceResponseAsync(servicePath, callParams, cacheKey, defaultCacheSeconds, responseMode,
                xmlParseDelegate);
        }

        /// <summary>
        ///     Initiates an async request to the service provider.
        /// </summary>
        /// <typeparam name="T">ApiType of the entity object to contain the response data.</typeparam>
        /// <param name="servicePath">relative url path to the service method.</param>
        /// <param name="callParams">A collection of name/value parameters to send on the request.</param>
        /// <param name="cacheKey">key used for caching data</param>
        /// <param name="defaultCacheSeconds">how long to cache data if service doesn't provide a value</param>
        /// <param name="responseMode"></param>
        /// <param name="xmlParseDelegate">the delegate for parsing the xml.</param>
        /// <returns>A reference to the async task.</returns>
        protected internal Task<EveServiceResponse<T>> GetServiceResponseAsync<T>(string servicePath,
            IDictionary<string, string> callParams, string cacheKey, int defaultCacheSeconds, ResponseMode responseMode,
            Func<XElement, T> xmlParseDelegate)
            where T : class
        {
            Uri temp;
            if (!Uri.TryCreate(_eveWebServiceLocation + servicePath, UriKind.Absolute, out temp))
            {
                throw new InvalidOperationException(
                    "\"{0}\" and \"{1}\" cannot be combined to form a proper Url".FormatInvariant(
                        _eveWebServiceLocation, servicePath));
            }

            // check cache for data and return if cached data exists and is still valid.
            CacheItem<EveServiceResponse<T>> resultData = null;

            if (responseMode != ResponseMode.BypassCache)
            {
                resultData = GetCacheEntry<T>(cacheKey);
            }

            Task<EveServiceResponse<T>> resultTask;

            if (responseMode == ResponseMode.CacheOnly)
            {
                if (resultData == null)
                {
                    resultData = new CacheItem<EveServiceResponse<T>>();
                    resultData.Data = new EveServiceResponse<T>();
                    resultData.CacheUntil = new DateTimeOffset();
                }

                resultTask = ReturnCachedResponse(resultData);
            }
            else
            {
                // If there is data in cache, check to see if is dirty (old). If it is not dirty return cache data
                // unless there response mode is set to bypass cache
                // If the data is dirty or is null or last retrieval wasn't successful... get fresh data from CCP.
                if (resultData != null && !resultData.IsDirty && responseMode != ResponseMode.BypassCache && resultData.Data.IsSuccess)
                {
                    resultTask = ReturnCachedResponse(resultData);
                }
                else
                {
                    resultTask =
                        _provider.PostAsync(temp, callParams)
                            .ContinueWith(
                                webTask =>
                                    ProcessServiceResponse(webTask, cacheKey, defaultCacheSeconds, xmlParseDelegate));
                }
            }

            return resultTask;
        }

        /// <summary>
        ///     Disposes the object and cleans up resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        ///     Gets the data from caching system if it exists.
        /// </summary>
        /// <typeparam name="T">ApiType of data.</typeparam>
        /// <param name="key">The key to retrieve</param>
        /// <returns>And instance of the service response object.</returns>
        protected CacheItem<EveServiceResponse<T>> GetCacheEntry<T>(string key) where T : class
        {
            return _cache.Get<EveServiceResponse<T>>(key);
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<TReturn>(Func<Task<EveServiceResponse<TReturn>>> apiTask)
            where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask();
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, TReturn>(
            Func<T1, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1) where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1);
            task.Wait();
            return task.Result;
        }


        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, TReturn>(
            Func<T1, T2, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2) where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2);
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, T3, TReturn>(
            Func<T1, T2, T3, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2, T3 param3)
            where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2, param3);
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, T3, T4, TReturn>(
            Func<T1, T2, T3, T4, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2, T3 param3, T4 param4)
            where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2, param3, param4);
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, T3, T4, T5, TReturn>(
            Func<T1, T2, T3, T4, T5, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2, T3 param3,
            T4 param4, T5 param5) where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2, param3, param4, param5);
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, T3, T4, T5, T6, TReturn>(
            Func<T1, T2, T3, T4, T5, T6, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2, T3 param3,
            T4 param4, T5 param5, T6 param6) where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2, param3, param4, param5, param6);
            task.Wait();
            return task.Result;
        }

        protected EveServiceResponse<TReturn> RunAsyncMethod<T1, T2, T3, T4, T5, T6, T7, TReturn>(
            Func<T1, T2, T3, T4, T5, T6, T7, Task<EveServiceResponse<TReturn>>> apiTask, T1 param1, T2 param2, T3 param3,
            T4 param4, T5 param5, T6 param6, T7 param7) where TReturn : class
        {
            Task<EveServiceResponse<TReturn>> task = apiTask(param1, param2, param3, param4, param5, param6, param7);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        ///     Gets the caching details from the service response if exists, or uses the provided defaults.
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
        ///     Gets the content of an HTTP Response and reads the content as XML.
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
        ///     Creates a TPL Task for passing cached responses back to callers.
        /// </summary>
        /// <typeparam name="T">ApiType of the data to be return</typeparam>
        /// <param name="cachedResult">cached data.</param>
        /// <returns>A TPL task.</returns>
        private static Task<EveServiceResponse<T>> ReturnCachedResponse<T>(CacheItem<EveServiceResponse<T>> cachedResult)
            where T : class
        {
            // cachedResult was found in cache.
            cachedResult.Data.CachedResponse = true;

            // return the task with the response value in it.
            return Task.Factory.StartNew(() => cachedResult.Data);
        }

        /// <summary>
        ///     Processes the response from the remote service
        /// </summary>
        /// <typeparam name="T">Data entity to use as the response type</typeparam>
        /// <param name="webTask">task that was used to call the web service.</param>
        /// <param name="cacheKey">cache key for caching the response.</param>
        /// <param name="defaultCacheSeconds">default cache time used if the service doesn't provide it.</param>
        /// <param name="parseXml">delegate for parsing the xml into the strong data type.</param>
        /// <returns>The strongly typed data response.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Required in order to pass exception information to callers.")]
        private EveServiceResponse<T> ProcessServiceResponse<T>(Task<HttpResponseMessage> webTask, string cacheKey,
            int defaultCacheSeconds, Func<XElement, T> parseXml)
            where T : class
        {
            Exception faultError = null;
            T result = default(T);
            DateTimeOffset cacheTime = DateTimeOffset.Now.AddSeconds(defaultCacheSeconds);
            int eveErrorCode = 0;
            string eveErrorText = string.Empty;
            if (webTask.Exception != null)
            {
                faultError = webTask.Exception;
                Trace.TraceWarning(faultError.FormatException());
            }

            if (faultError == null)
            {
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
                    else if (xml != null && xml.Root != null && (resultsElement = xml.Root.Element(Error)) != null)
                    {
                        // CCP server side error
                        eveErrorCode = GetErrorCodeFromResponse(resultsElement, ref eveErrorText);

                        // because cacheTime in the response is bugged (per http://wiki.eve-id.net/APIv2_Eve_ErrorList_XML ), 
                        // defaulting cacheTime for 1 hour
                        cacheTime = DateTimeOffset.Now.AddHours(1);
                    }
                    else
                    {
                        // shenanigans.
                        eveErrorCode = 999999;
                        eveErrorText = "Unknown Error.";
                    }
                }
                catch (Exception e)
                {
                    // catch any of the xml processing errors
                    faultError = e;
                }
            }

            // store it.
            var eveResult = new EveServiceResponse<T>
            {
                ResultData = result,
                ServiceException = faultError,
                IsSuccessfulHttpStatus = faultError == null && webTask.Result.IsSuccessStatusCode,
                HttpStatusCode = faultError == null ? webTask.Result.StatusCode : HttpStatusCode.NotFound,
                CacheUntil = cacheTime,
                EveErrorCode = eveErrorCode,
                EveErrorText = eveErrorText
            };

            if (faultError == null)
            {
                // cache it
                SetCacheEntry(cacheKey, eveResult);
            }

            return eveResult;
        }

        private int GetErrorCodeFromResponse(XElement errorElement, ref string eveErrorText)
        {
            int errorCode = errorElement.Attribute("code").Value.ToInt32();
            eveErrorText = errorElement.Value;
            return errorCode;
        }

        /// <summary>
        ///     Sets data into cache.
        /// </summary>
        /// <typeparam name="T">type of data being stored.</typeparam>
        /// <param name="key">key to store the data under.</param>
        /// <param name="data">date to store.</param>
        private void SetCacheEntry<T>(string key, EveServiceResponse<T> data) where T : class
        {
            _cache.Add(key, data, data.CacheUntil);
        }
    }
}