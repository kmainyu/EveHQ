// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (ContractItem.cs), is part of EveHQ.
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
    /// <summary>The contract item.</summary>
    public class ContractItem
    {
        /// <summary>Gets or sets the record id.</summary>
        public long RecordId { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        public int TypeId { get; set; }

        /// <summary>Gets or sets the quantity.</summary>
        public long Quantity { get; set; }

        /// <summary>Gets or sets the raw quantity.</summary>
        public int RawQuantity { get; set; }

        /// <summary>Gets or sets a value indicating whether is singleton.</summary>
        public bool IsSingleton { get; set; }

        /// <summary>Gets or sets a value indicating whether is included.</summary>
        public bool IsIncluded { get; set; }
    }
}