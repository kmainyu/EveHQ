// ===========================================================================
// <copyright file="MarketOrder.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (MarketOrder.cs), is part of EveHQ.
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

    /// <summary>
    ///     Market object used to describe an single order (buy or sell) for an item
    /// </summary>
    public class MarketOrder
    {
        #region Public Properties

        /// <summary>Gets or sets the duration.</summary>
        public int Duration { get; set; }

        /// <summary>Gets or sets the expires.</summary>
        public DateTimeOffset Expires { get; set; }

        /// <summary>Gets or sets the freshness.</summary>
        public DateTimeOffset Freshness { get; set; }

        /// <summary>Gets or sets a value indicating whether is buy order.</summary>
        public bool IsBuyOrder { get; set; }

        /// <summary>Gets or sets the issued.</summary>
        public DateTimeOffset Issued { get; set; }

        /// <summary>Gets or sets the item id.</summary>
        public int ItemId { get; set; }

        /// <summary>Gets or sets the jumps.</summary>
        public int Jumps { get; set; }

        /// <summary>Gets or sets the min quantity.</summary>
        public int MinQuantity { get; set; }

        /// <summary>Gets or sets the order id.</summary>
        public long OrderId { get; set; }

        /// <summary>Gets or sets the order range.</summary>
        public int OrderRange { get; set; }

        /// <summary>Gets or sets the price.</summary>
        public double Price { get; set; }

        /// <summary>Gets or sets the quantity entered.</summary>
        public int QuantityEntered { get; set; }

        /// <summary>Gets or sets the quantity remaining.</summary>
        public int QuantityRemaining { get; set; }

        /// <summary>Gets or sets the region id.</summary>
        public int RegionId { get; set; }

        /// <summary>Gets or sets the security.</summary>
        public double Security { get; set; }

        /// <summary>Gets or sets the solar system id.</summary>
        public int SolarSystemId { get; set; }

        /// <summary>Gets or sets the station id.</summary>
        public int StationId { get; set; }

        /// <summary>Gets or sets the station name.</summary>
        public string StationName { get; set; }

        #endregion
    }
}