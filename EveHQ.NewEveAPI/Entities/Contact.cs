// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (Contact.cs), is part of EveHQ.
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
    using Newtonsoft.Json;

    /// <summary>
    /// A Contact in EVE that is part of a Contact List.
    /// </summary>
    public sealed class Contact
    {
        /// <summary>Gets the contact id.</summary>
        public int ContactId { get; set; }

        /// <summary>Gets the contact name.</summary>
        public string ContactName { get; set; }

        public ContactType ContactType { get; set; }

        /// <summary>Gets a value indicating whether is in watch list.</summary>
        public bool IsInWatchList { get; set; }

        /// <summary>Gets the standing.</summary>
        public int Standing { get; set; }
    }

    public enum ContactType
    {
        ContactList,
        CorporateContactList,
        AllianceContactList
    }
}