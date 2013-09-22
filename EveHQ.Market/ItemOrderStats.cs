// ===========================================================================
// <copyright file="ItemOrderStats.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ItemOrderStats.cs), is part of EveHQ.
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
    /// <summary>
    ///     Market stats data object for an item in Eve
    /// </summary>
    public class ItemOrderStats
    {
        #region Public Properties

        /// <summary>Gets or sets all of the orders.</summary>
        public OrderStats All { get; set; }

        /// <summary>Gets or sets the buy orders.</summary>
        public OrderStats Buy { get; set; }

        /// <summary>Gets or sets the item type id.</summary>
        public int ItemTypeId { get; set; }

        /// <summary>Gets or sets the sell orders.</summary>
        public OrderStats Sell { get; set; }

        #endregion
    }
}