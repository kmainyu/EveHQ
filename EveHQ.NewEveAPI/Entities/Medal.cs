// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (Medal.cs), is part of EveHQ.
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

    /// <summary>
    /// Describes a medal assigned to the character.
    /// </summary>
    public sealed class Medal
    {
       
        /// <summary>Gets the medal id.</summary>
        public int MedalId { get; set; }

        /// <summary>Gets the reason.</summary>
        public string Reason { get; set; }

        /// <summary>Gets the status.</summary>
        public string Status { get; set; }

        /// <summary>Gets the issuer id.</summary>
        public int IssuerId { get; set; }

        /// <summary>Gets the date issued.</summary>
        public DateTimeOffset DateIssued { get; set; }

        /// <summary>Gets the corporation id.</summary>
        public int CorporationId { get; set; }

        /// <summary>Gets the title.</summary>
        public string Title { get; set; }

        /// <summary>Gets the description.</summary>
        public string Description { get; set; }
    }
}