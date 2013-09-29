//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (AccountCharacter.cs), is part of EveHQ.
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
    /// Describes a single Eve Online Character in an Account
    /// </summary>
    public sealed class AccountCharacter
    {
        /// <summary>
        /// Gets the character's ID.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets the character's corporation id.
        /// </summary>
        public int CorporationId { get; set; }

        /// <summary>
        /// Gets the character's corporation name.
        /// </summary>
        public string CorporationName { get; set; }

        /// <summary>
        /// Gets the character's name.
        /// </summary>
        public string Name { get; set; }
    }
}