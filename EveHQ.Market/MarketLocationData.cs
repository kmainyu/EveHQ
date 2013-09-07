// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (MarketLocationData.cs), is part of EveHQ.
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
namespace EveHQ.Market
{
    using System;

    /// <summary>The market location data.</summary>
    public class MarketLocationData
    {
        /// <summary>Gets or sets the location id.</summary>
        public int LocationId { get; set; }

        /// <summary>Gets or sets the freshness.</summary>
        public DateTimeOffset Freshness { get; set; }

        /// <summary>Gets or sets the path from root.</summary>
        public string PathFromRoot { get; set; }
    }
}