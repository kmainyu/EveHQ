// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (CharacterSkillRecord.cs), is part of EveHQ.
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

    /// <summary>The character skill item.</summary>
    public sealed class CharacterSkillRecord
    {
        /// <summary>Gets the skill id.</summary>
        public int SkillId { get; set; }

        /// <summary>Gets the skill points.</summary>
        public int SkillPoints { get; set; }

        /// <summary>Gets the level.</summary>
        public int Level { get; set; }

        /// <summary>Gets a value indicating whether published.</summary>
        public bool Published { get; set; }
    }
}