// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (IMarketDataProvider.cs), is part of EveHQ.
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
namespace EveHQ.Market
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the functional contract for an object that wishes to provide Eve Market Data for items.
    /// </summary>
    public interface IMarketStatDataProvider
    {
        string ProviderName { get; }

        bool LimitedSystemSelection { get; }
        IEnumerable<int> SupportedSystems { get; }

        bool LimitedRegionSelection { get; }
        IEnumerable<int> SupportedRegions { get; }
        
        /// <summary>Get order data by regions.</summary>
        /// <param name="typeIds">The type ids.</param>
        /// <param name="includeRegions">The include regions.</param>
        /// <param name="minQuantity">The min quantity.</param>
        /// <returns>The <see cref="IAsyncResult"/>.</returns>
        Task<IEnumerable<ItemOrderStats>> GetOrderStats(IEnumerable<int> typeIds, IEnumerable<int> includeRegions, int? systemId, int minQuantity);
    }
}