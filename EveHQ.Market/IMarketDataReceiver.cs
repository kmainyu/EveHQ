// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (IMarketDataReceiver.cs), is part of EveHQ.
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
    using System.Threading.Tasks;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMarketDataReceiver
    {
        /// <summary>Gets a value indicating whether is enabled.</summary>
        bool IsEnabled { get; }

        /// <summary>Gets the next attempt.</summary>
        DateTimeOffset NextAttempt { get; }

        /// <summary>Gets the unified upload key.</summary>
        UploadKey UnifiedUploadKey { get; }

        /// <summary>The upload market data.</summary>
        /// <param name="marketDataJson">The market data json.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UploadMarketData(string marketDataJson);
    }
}