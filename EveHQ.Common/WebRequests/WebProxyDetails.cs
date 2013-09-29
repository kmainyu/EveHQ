// ===========================================================================
// <copyright file="WebProxyDetails.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (WebProxyDetails.cs), is part of EveHQ.
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

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WebProxyDetails
    {
        #region Public Properties

        /// <summary>Gets or sets the proxy password.</summary>
        public string ProxyPassword { get; set; }

        /// <summary>Gets or sets the proxy server address.</summary>
        public Uri ProxyServerAddress { get; set; }

        /// <summary>Gets or sets the proxy user name.</summary>
        public string ProxyUserName { get; set; }

        /// <summary>Gets or sets a value indicating whether use basic auth.</summary>
        public bool UseBasicAuth { get; set; }

        /// <summary>Gets or sets a value indicating whether use default credential.</summary>
        public bool UseDefaultCredential { get; set; }

        #endregion
    }
}