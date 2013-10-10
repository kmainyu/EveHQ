// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (AssetItem.cs), is part of EveHQ.
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
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The asset item.
    /// </summary>
    public sealed class AssetItem
    {
        /// <summary>
        /// Gets the ID value of the Item. Only guaranteed unique at the time of the asset load.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets the Location (sol system or star base) of the item. Not used with items that are inside a container item.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets the item Type Id. This id can be used to get details from the inventory types Table of the EveDB.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets how many of this item there are.
        /// </summary>
        public long Quantity { get; set; }

        /// <summary>
        /// Gets the flag value for the item.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag", Justification = "It's the name of the field returned from Eve")]
        public int Flag { get; set; }

        /// <summary>
        /// Gets a value indicating whether this item is a singleton or not.
        /// </summary>
        public bool Singleton { get; set; }

        /// <summary>
        /// Gets the contents of this item.
        /// </summary>
        public IEnumerable<AssetItem> Contents { get; set; }

        /// <summary>
        /// Gets the containing item, if this item is inside a container.
        /// </summary>
        public int ParentItemId { get; set; }
    }
}