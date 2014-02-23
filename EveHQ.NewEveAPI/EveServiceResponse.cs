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

namespace EveHQ.EveApi
{
    /// <summary>
    ///     Describes the response data from a call to the EveAPI Service.
    /// </summary>
    /// <typeparam name="T">Object for the response data.</typeparam>
    public sealed class EveServiceResponse<T> where T : class
    {
        /// <summary>
        ///     Gets the date and time of when this response should be expired from cache.
        /// </summary>
        public DateTimeOffset CacheUntil { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance was retrieved from cache.
        /// </summary>
        public bool CachedResponse { get; set; }

        /// <summary>
        ///     Gets the data for the response.
        /// </summary>
        public T ResultData { get; set; }

        /// <summary>
        ///     Gets the exception related to this service response.
        /// </summary>
        public Exception ServiceException { get; set; }

        /// <summary>
        ///     Gets a value indicating whether there was an exception thrown during processing.
        /// </summary>
        public bool IsFaulted
        {
            get { return ServiceException != null; }
        }

        /// <summary>
        ///     Gets a value indicating whether the call was successful (from an HTTP status POV).
        /// </summary>
        public bool IsSuccessfulHttpStatus { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public bool IsSuccess
        {
            get
            {
                return IsSuccessfulHttpStatus && !IsFaulted && ServiceException == null && EveErrorCode == 0 &&
                       string.IsNullOrWhiteSpace(EveErrorText) && ResultData != null;
            }
        }

        public int EveErrorCode { get; set; }

        public string EveErrorText { get; set; }
    }
}