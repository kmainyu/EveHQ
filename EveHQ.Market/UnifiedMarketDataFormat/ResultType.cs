// ===========================================================================
// <copyright file="ResultType.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ResultType.cs), is part of EveHQ.
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
    using System;

    using Newtonsoft.Json;

    /// <summary>
    ///     The kind of market data in the rowset.
    /// </summary>
    [JsonConverter(typeof(ResultKindConverter))]
    public enum ResultType
    {
        /// <summary>The orders.</summary>
        Orders, 

        /// <summary>The history.</summary>
        History
    }
}