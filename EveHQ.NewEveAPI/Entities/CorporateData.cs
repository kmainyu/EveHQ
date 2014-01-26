// ===========================================================================
// <copyright file="CorporateData.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (CorporateData.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.EveApi
{
    using System.Collections.Generic;

    /// <summary>The corporate data.</summary>
    public class CorporateData
    {
        /// <summary>Gets or sets the corporation id.</summary>
        public int CorporationId { get; set; }

        /// <summary>Gets or sets the corporation name.</summary>
        public string CorporationName { get; set; }

        /// <summary>Gets or sets the ticker.</summary>
        public string Ticker { get; set; }

        /// <summary>Gets or sets the ceo id.</summary>
        public int CeoId { get; set; }

        /// <summary>Gets or sets the ceo name.</summary>
        public string CeoName { get; set; }

        /// <summary>Gets or sets the station id.</summary>
        public int StationId { get; set; }

        /// <summary>Gets or sets the station name.</summary>
        public string StationName { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the url.</summary>
        public string Url { get; set; }

        /// <summary>Gets or sets the alliance id.</summary>
        public int AllianceId { get; set; }

        /// <summary>Gets or sets the alliance name.</summary>
        public string AllianceName { get; set; }

        /// <summary>Gets or sets the tax rate.</summary>
        public string TaxRate { get; set; }

        /// <summary>Gets or sets the member count.</summary>
        public int MemberCount { get; set; }

        /// <summary>Gets or sets the member limit.</summary>
        public int MemberLimit { get; set; }

        /// <summary>Gets or sets the shares.</summary>
        public int Shares { get; set; }

        /// <summary>Gets or sets the divisions.</summary>
        public IEnumerable<CorporateDivision> Divisions { get; set; }

        /// <summary>Gets or sets the wallet divisions.</summary>
        public IEnumerable<CorporateDivision> WalletDivisions { get; set; }

        public CorporateLogo Logo { get; set; }
    }

    /// <summary>The corporate division.</summary>
    public class CorporateDivision
    {
        /// <summary>Gets or sets the account key.</summary>
        public int AccountKey { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; }
    }

    /// <summary>The corporate logo.</summary>
    public class CorporateLogo
    {
        /// <summary>Gets or sets the graphic id.</summary>
        public int GraphicId { get; set; }

        /// <summary>Gets or sets the shape 1.</summary>
        public int Shape1 { get; set; }

        /// <summary>Gets or sets the shape 2.</summary>
        public int Shape2 { get; set; }

        /// <summary>Gets or sets the shape 3.</summary>
        public int Shape3 { get; set; }

        /// <summary>Gets or sets the color 1.</summary>
        public int Color1 { get; set; }

        /// <summary>Gets or sets the color 2.</summary>
        public int Color2 { get; set; }

        /// <summary>Gets or sets the color 3.</summary>
        public int Color3 { get; set; }
    }
}