﻿// ===========================================================================
// <copyright file="MarketTransactionKind.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (MarketTransactionKind.cs), is part of EveHQ.
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
    ///     The type of Transaction
    /// </summary>
    public enum MarketTransactionKind
    {
        /// <summary>The sell.</summary>
        Sell, 

        /// <summary>The buy.</summary>
        Buy, 

        /// <summary>The all.</summary>
        All, 

        /// <summary>The default.</summary>
        Default
    }
}