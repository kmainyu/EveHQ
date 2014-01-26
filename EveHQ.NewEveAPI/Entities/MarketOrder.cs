// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (MarketOrder.cs), is part of EveHQ.
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
    using System;

    /// <summary>
    /// A single Market order
    /// </summary>
    public sealed class MarketOrder
    {
       

        /// <summary>Gets the order id.</summary>
        public int OrderId { get; set; }

        /// <summary>Gets the char id.</summary>
        public int CharId { get; set; }

        /// <summary>Gets the station id.</summary>
        public int StationId { get; set; }

        /// <summary>Gets the quantity entered.</summary>
        public int QuantityEntered { get; set; }

        /// <summary>Gets the quantity remaining.</summary>
        public int QuantityRemaining { get; set; }

        /// <summary>Gets the min quantity.</summary>
        public int MinQuantity { get; set; }

        /// <summary>Gets the order state.</summary>
        public MarketOrderState OrderState { get; set; }

        /// <summary>Gets the type id.</summary>
        public int TypeId { get; set; }

        /// <summary>Gets the range.</summary>
        public int Range { get; set; }

        /// <summary>Gets the account key.</summary>
        public int AccountKey { get; set; }

        /// <summary>Gets the duration.</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Gets the escrow.</summary>
        public double Escrow { get; set; }

        /// <summary>Gets the price.</summary>
        public double Price { get; set; }

        /// <summary>Gets a value indicating whether is buy order.</summary>
        public bool IsBuyOrder { get; set; }

        /// <summary>Gets the date issued.</summary>
        public DateTimeOffset DateIssued { get; set; }
    }
}