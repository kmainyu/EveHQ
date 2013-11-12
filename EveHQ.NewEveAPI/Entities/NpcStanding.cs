// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (NpcStanding.cs), is part of EveHQ.
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
    /// <summary>The NPC standing.</summary>
    public sealed class NpcStanding
    {
        /// <summary>Gets or sets the kind.</summary>
        public NpcType Kind { get; set; }

        /// <summary>Gets or sets the from id.</summary>
        public long FromId { get; set; }

        /// <summary>Gets or sets the from name.</summary>
        public string FromName { get; set; }

        /// <summary>Gets or sets the standing.</summary>
        public double Standing { get; set; }
    }

    /// <summary>The NPC type.</summary>
    public enum NpcType
    {
        /// <summary>The agents.</summary>
        Agents,

        /// <summary>The NPC corporations.</summary>
        NPCCorporations,

        /// <summary>The factions.</summary>
        Factions
    }
}