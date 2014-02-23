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

using System.Collections.Generic;

namespace EveHQ.EveApi
{
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