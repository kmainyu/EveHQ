// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (UnifiedDataRowset.cs), is part of EveHQ.
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
    using System.Collections.Generic;

    /// <summary>TODO: Update summary.</summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnifiedDataRowset<T>
    {
        /// <summary>Gets or sets the generated at.</summary>
        public string GeneratedAt { get; set; }

        /// <summary>Gets or sets the region id.</summary>
        public string RegionID { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        public string TypeID { get; set; }

        /// <summary>Gets or sets the rows.</summary>
        public List<T> Rows { get; set; }
    }
}