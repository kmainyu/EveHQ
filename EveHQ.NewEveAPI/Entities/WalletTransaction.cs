// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (WalletTransaction.cs), is part of EveHQ.
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

    /// <summary>The wallet transaction.</summary>
    public class WalletTransaction
    {
        /// <summary>Gets or sets the transaction date time.</summary>
        public DateTimeOffset TransactionDateTime { get; set; }

        /// <summary>Gets or sets the transaction id.</summary>
        public int TransactionId { get; set; }

        /// <summary>Gets or sets the quantity.</summary>
        public int Quantity { get; set; }

        /// <summary>Gets or sets the type name.</summary>
        public string TypeName { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        public int TypeId { get; set; }

        /// <summary>Gets or sets the price.</summary>
        public double Price { get; set; }

        /// <summary>Gets or sets the client id.</summary>
        public int ClientId { get; set; }

        /// <summary>Gets or sets the client name.</summary>
        public string ClientName { get; set; }

        /// <summary>Gets or sets the station id.</summary>
        public int StationId { get; set; }

        /// <summary>Gets or sets the station name.</summary>
        public string StationName { get; set; }

        /// <summary>Gets or sets the transaction type.</summary>
        public string TransactionType { get; set; }

        /// <summary>Gets or sets the transaction for.</summary>
        public string TransactionFor { get; set; }

        /// <summary>Gets or sets the wallet journal entry id.</summary>
        public long WalletJournalEntryId { get; set; }
    }
}