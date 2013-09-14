// ===========================================================================
// <copyright file="HistoryRow.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (HistoryRow.cs), is part of EveHQ.
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
namespace EveHQ.Market.UnifiedMarketDataFormat
{
    using Newtonsoft.Json;

    /// <summary>
    ///     Order data for an item on a particular date.
    /// </summary>
    [JsonConverter(typeof(HistoryRowConverter))]
    public class HistoryRow
    {
        #region Public Properties

        /// <summary>Gets or sets the average value.</summary>
        public double Average { get; set; }

        /// <summary>Gets or sets the date.</summary>
        public string Date { get; set; }

        /// <summary>Gets or sets the highest value.</summary>
        public double High { get; set; }

        /// <summary>Gets or sets the lowest value.</summary>
        public double Low { get; set; }

        /// <summary>Gets or sets the number of orders.</summary>
        public long Orders { get; set; }

        /// <summary>Gets or sets the quantity of items sold.</summary>
        public long Quantity { get; set; }

        #endregion
    }
}