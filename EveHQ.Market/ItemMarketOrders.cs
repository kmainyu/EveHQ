﻿// ===========================================================================
// <copyright file="ItemMarketOrders.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ItemMarketOrders.cs), is part of EveHQ.
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
namespace EveHQ.Market
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Market Orders for an item.
    /// </summary>
    public class ItemMarketOrders
    {
        #region Public Properties

        /// <summary>Gets or sets the buy orders.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Affects being read from XML")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for converting to/from JSON")]
        public List<MarketOrder> BuyOrders { get; set; }

        /// <summary>Gets or sets the hours.</summary>
        public int Hours { get; set; }

        /// <summary>Gets or sets the item name.</summary>
        public string ItemName { get; set; }

        /// <summary>Gets or sets the item type id.</summary>
        public int ItemTypeId { get; set; }

        /// <summary>Gets or sets the min quantity.</summary>
        public int MinQuantity { get; set; }

        /// <summary>Gets or sets the regions.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for converting to/from JSON")]
        public HashSet<int> Regions { get; set; }

        /// <summary>Gets or sets the sell orders.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Affects being read from XML")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for converting to/from JSON")]
        public List<MarketOrder> SellOrders { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp of the data.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        #endregion
    }
}