//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (EveServiceResponse.cs), is part of EveHQ.
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
    using System.Net;

    /// <summary>
    /// Describes the response data from a call to the EveAPI Service.
    /// </summary>
    /// <typeparam name="T">Object for the response data.</typeparam>
    public sealed class EveServiceResponse<T>
    {
        /// <summary>
        /// Gets the date and time of when this response should be expired from cache.
        /// </summary>
        public DateTimeOffset CacheUntil { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance was retrieved from cache.
        /// </summary>
        public bool CachedResponse { get; set; }

        /// <summary>
        /// Gets the data for the response.
        /// </summary>
        public T ResultData { get; set; }

        /// <summary>
        /// Gets the exception related to this service response.
        /// </summary>
        public Exception ServiceException { get;  set; }

        /// <summary>
        /// Gets a value indicating whether there was an exception thrown during processing.
        /// </summary>
        public bool IsFaulted
        {
            get
            {
                return ServiceException != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the call was successful (from an HTTP status POV).
        /// </summary>
        public bool IsSuccessfulHttpStatus { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public int EveErrorCode { get; set; }

        public string EveErrorText { get; set; }
    }
}