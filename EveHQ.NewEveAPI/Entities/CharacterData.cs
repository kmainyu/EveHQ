// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (CharacterData.cs), is part of EveHQ.
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
    using System.Collections.Generic;

    /// <summary>The character data.</summary>
    public sealed class CharacterData
    {
        /// <summary>Gets the character id.</summary>
        public int CharacterId { get; set; }

        /// <summary>Gets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets the birth date.</summary>
        public DateTimeOffset BirthDate { get; set; }

        /// <summary>Gets the race.</summary>
        public string Race { get; set; }

        /// <summary>Gets the blood line.</summary>
        public string BloodLine { get; set; }

        /// <summary>Gets the ancestry.</summary>
        public string Ancestry { get; set; }

        /// <summary>Gets the gender.</summary>
        public string Gender { get; set; }

        /// <summary>Gets the corporation name.</summary>
        public string CorporationName { get; set; }

        /// <summary>Gets the corporation id.</summary>
        public int CorporationId { get; set; }

        /// <summary>Gets the alliance name.</summary>
        public string AllianceName { get; set; }

        /// <summary>Gets the alliance id.</summary>
        public int AllianceId { get; set; }

        /// <summary>Gets the clone name.</summary>
        public string CloneName { get; set; }

        /// <summary>Gets the clone skill points.</summary>
        public int CloneSkillPoints { get; set; }

        /// <summary>Gets the balance.</summary>
        public double Balance { get; set; }

        /// <summary>Gets the memory bonus.</summary>
        public AttributeEnhancer MemoryBonus { get; set; }

        /// <summary>Gets the perception bonus.</summary>
        public AttributeEnhancer PerceptionBonus { get; set; }

        /// <summary>Gets the willpower bonus.</summary>
        public AttributeEnhancer WillpowerBonus { get; set; }

        /// <summary>Gets the intelligence bonus.</summary>
        public AttributeEnhancer IntelligenceBonus { get; set; }

        /// <summary>Gets the charisma bonus.</summary>
        public AttributeEnhancer CharismaBonus { get; set; }

        /// <summary>Gets the intelligence.</summary>
        public int Intelligence { get; set; }

        /// <summary>Gets the memory.</summary>
        public int Memory { get; set; }

        /// <summary>Gets the charisma.</summary>
        public int Charisma { get; set; }

        /// <summary>Gets the perception.</summary>
        public int Perception { get; set; }

        /// <summary>Gets the willpower.</summary>
        public int Willpower { get; set; }

        /// <summary>Gets the skills.</summary>
        public IEnumerable<CharacterSkillRecord> Skills { get; set; }

        /// <summary>Gets the certificates.</summary>
        public IEnumerable<int> Certificates { get; set; }

        /// <summary>Gets the corporation roles.</summary>
        public IEnumerable<CharacterCorporationRoles> CorporationRoles { get; set; }

        /// <summary>Gets the corporation roles at HQ.</summary>
        public IEnumerable<CharacterCorporationRoles> CorporationRolesAtHq { get; set; }

        /// <summary>Gets the corporation roles at base.</summary>
        public IEnumerable<CharacterCorporationRoles> CorporationRolesAtBase { get; set; }

        /// <summary>Gets the corporation roles at others.</summary>
        public IEnumerable<CharacterCorporationRoles> CorporationRolesAtOthers { get; set; }

        /// <summary>Gets the corporation titles.</summary>
        public IEnumerable<CharacterCorporationTitles> CorporationTitles { get; set; }
    }
}