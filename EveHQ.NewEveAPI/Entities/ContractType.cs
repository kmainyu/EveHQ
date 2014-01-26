// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (ContractType.cs), is part of EveHQ.
// 
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
// 
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================

namespace EveHQ.EveApi
{
    /// <summary>Defines the type of a specific contract.</summary>
    public enum ContractType
    {
        /// <summary>The unknown.</summary>
        Unknown,

        /// <summary>The item exchange.</summary>
        ItemExchange,

        /// <summary>The courier.</summary>
        Courier,

        /// <summary>The loan.</summary>
        Loan,

        /// <summary>The auction.</summary>
        Auction
    }
}