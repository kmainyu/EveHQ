// ===========================================================================
// <copyright file="OrderRow.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (OrderRow.cs), is part of EveHQ.
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
    ///     Data for a single market order.
    /// </summary>
    [JsonConverter(typeof(OrderRowConverter))]
    public class OrderRow
    {
        #region Public Properties

        /// <summary>Gets or sets a value indicating whether bid.</summary>
        public bool Bid { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        public long Duration { get; set; }

        /// <summary>Gets or sets the issue date.</summary>
        public string IssueDate { get; set; }

        /// <summary>Gets or sets the min volume.</summary>
        public long MinVolume { get; set; }

        /// <summary>Gets or sets the order id.</summary>
        public long OrderId { get; set; }

        /// <summary>Gets or sets the price.</summary>
        public double Price { get; set; }

        /// <summary>Gets or sets the range.</summary>
        public long Range { get; set; }

        /// <summary>Gets or sets the solar system id.</summary>
        public long SolarSystemId { get; set; }

        /// <summary>Gets or sets the station id.</summary>
        public long StationId { get; set; }

        /// <summary>Gets or sets the original number of items to be sold.</summary>
        public long VolEntered { get; set; }

        /// <summary>Gets or sets the number of remaining items.</summary>
        public long VolRemaining { get; set; }

        #endregion
    }
}