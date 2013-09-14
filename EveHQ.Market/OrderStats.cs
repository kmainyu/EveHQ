// ===========================================================================
// <copyright file="OrderStats.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (OrderStats.cs), is part of EveHQ.
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
    ///     Used by ItemOrderStats to store data pertaining to buy/sell/ all
    /// </summary>
    public class OrderStats
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the Average value
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        ///     Gets or sets  the Maximum value
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        ///     Gets or sets  the Median value.
        /// </summary>
        public double Median { get; set; }

        /// <summary>
        ///     Gets or sets  the Minimum value
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        ///     Gets or sets  the percentile value.
        /// </summary>
        public double Percentile { get; set; }

        /// <summary>
        ///     Gets or sets  the standard deviation
        /// </summary>
        public double StdDeviation { get; set; }

        /// <summary>
        ///     Gets or sets  the number of items in this data.
        /// </summary>
        public long Volume { get; set; }

        #endregion
    }
}