//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (Account.cs), is part of EveHQ.
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
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Describes the status of a given account
    /// </summary>
    public sealed class Account
    {
       
        /// <summary>
        /// Gets the date of creation
        /// </summary>
        public DateTimeOffset CreateDate { get;  set; }

        /// <summary>
        /// Gets the Expiry date of the account.
        /// </summary>
        public DateTimeOffset ExpiryDate { get;  set; }

        /// <summary>
        /// Gets the number of times the account as logged in.
        /// </summary>
        public int LogOnCount { get;  set; }

        /// <summary>
        /// Gets the total amount of time (to the nearest minute) the account has been logged in.
        /// </summary>
        public TimeSpan LoggedInTime { get;  set; }

        /// <summary>
        /// Gets the Id of the account.
        /// </summary>
        public int UserId { get;  set; }
    }
}