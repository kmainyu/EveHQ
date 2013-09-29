// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (WalletJournalEntry.cs), is part of EveHQ.
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

    /// <summary>The wallet journal entry.</summary>
    public class WalletJournalEntry
    {
        /// <summary>Gets or sets the date.</summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>Gets or sets the ref id.</summary>
        public long RefId { get; set; }

        /// <summary>Gets or sets the reference type.</summary>
        public int ReferenceType { get; set; }

        /// <summary>Gets or sets the first party name.</summary>
        public string FirstPartyName { get; set; }

        /// <summary>Gets or sets the first party id.</summary>
        public int FirstPartyId { get; set; }

        /// <summary>Gets or sets the second party name.</summary>
        public string SecondPartyName { get; set; }

        /// <summary>Gets or sets the second party id.</summary>
        public int SecondPartyId { get; set; }

        /// <summary>Gets or sets the argument name.</summary>
        public string ArgumentName { get; set; }

        /// <summary>Gets or sets the argument id.</summary>
        public int ArgumentId { get; set; }

        /// <summary>Gets or sets the amount.</summary>
        public double Amount { get; set; }

        /// <summary>Gets or sets the balance.</summary>
        public double Balance { get; set; }

        /// <summary>Gets or sets the reason.</summary>
        public string Reason { get; set; }

        /// <summary>Gets or sets the tax receiver id.</summary>
        public int TaxReceiverId { get; set; }

        /// <summary>Gets or sets the tax amount.</summary>
        public double TaxAmount { get; set; }
    }
}