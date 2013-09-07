// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (HistoryRowset.cs), is part of EveHQ.
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
namespace EveHQ.Market.UnifiedMarketDataFormat
{
    using System.Collections.Generic;

    /// <summary>
    /// Strongly typed history row set.
    /// </summary>
    public class HistoryRowset : UnifiedDataRowset<HistoryRow>
    {
        /// <summary>The column names.</summary>
        private static readonly string[] Columns = new[] { "date", "orders", "quantity", "low", "high", "average" };

        /// <summary>Initializes a new instance of the <see cref="HistoryRowset"/> class.</summary>
        public HistoryRowset()
        {
            Rows = new List<HistoryRow>();
        }

        /// <summary>Gets the column names.</summary>
        public static string[] ColumnNames
        {
            get
            {
                return Columns;
            }
        }
    }
}