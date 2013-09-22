// ===========================================================================
// <copyright file="IMarketOrderDataProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (IMarketOrderDataProvider.cs), is part of EveHQ.
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    ///     Defines the functional contract for retrieving current market order results.
    /// </summary>
    public interface IMarketOrderDataProvider
    {
        #region Public Methods and Operators

        /// <summary>The get market orders for item type.</summary>
        /// <param name="itemTypeId">The item type id.</param>
        /// <param name="includedRegions">The included regions.</param>
        /// <param name="systemId">The system id.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<ItemMarketOrders> GetMarketOrdersForItemType(int itemTypeId, IEnumerable<int> includedRegions, int? systemId, int minQuantity);

        #endregion
    }
}