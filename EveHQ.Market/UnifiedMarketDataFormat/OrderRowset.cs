// ===========================================================================
// <copyright file="OrderRowset.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (OrderRowset.cs), is part of EveHQ.
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
    using System.Collections.Generic;

    /// <summary>
    ///     Strongly typed order history row.
    /// </summary>
    public class OrderRowset : UnifiedDataRowset<OrderRow>
    {
        #region Static Fields

        /// <summary>The _column names.</summary>
        private static readonly string[] Columns = new[] { "price", "volRemaining", "range", "orderID", "volEntered", "minVolume", "bid", "issueDate", "duration", "stationID", "solarSystemID" };

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="OrderRowset" /> class.</summary>
        public OrderRowset()
        {
            Rows = new List<OrderRow>();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the column names.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "used for serialization formatting to JSON.")]
        public static string[] ColumnNames
        {
            get
            {
                return Columns;
            }
        }

        #endregion
    }
}