// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (ContractStatus.cs), is part of EveHQ.
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
    /// <summary>
    /// Status values of a contract
    /// </summary>
    public enum ContractStatus
    {
        /// <summary>The unknown.</summary>
        Unknown,

        /// <summary>The outstanding.</summary>
        Outstanding,

        /// <summary>The deleted.</summary>
        Deleted,

        /// <summary>The completed.</summary>
        Completed,

        /// <summary>The failed.</summary>
        Failed,

        /// <summary>The completed by issuer.</summary>
        CompletedByIssuer,

        /// <summary>The completed by contractor.</summary>
        CompletedByContractor,

        /// <summary>The cancelled.</summary>
        Cancelled,

        /// <summary>The rejected.</summary>
        Rejected,

        /// <summary>The reversed.</summary>
        Reversed,

        /// <summary>The in progress.</summary>
        InProgress
    }
}