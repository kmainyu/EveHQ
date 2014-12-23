// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System;
using System.Collections.Generic;

namespace EveHQ.EveApi
{
    /// <summary>The character data.</summary>
    public sealed class CharacterData
    {
        /// <summary>Gets the character id.</summary>
        public int CharacterId { get; set; }

        /// <summary>Gets the name.</summary>
        public string Name { get; set; }

        /// <summary> Gets the home station ID.</summary>
        public int HomeStationId { get; set; }

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

        /// <summary>Gets the free skill points.</summary>
        public int FreeSkillPoints { get; set; }

        /// <summary>Gets the free respecs.</summary>
        public int FreeRespecs { get; set; }

        /// <summary>Gets the last clone jump date.</summary>
        public DateTimeOffset CloneJumpDate { get; set; }

        /// <summary>Gets the last respec date.</summary>
        public DateTimeOffset LastRespecDate { get; set; }

        /// <summary>Gets the last timed respec date.</summary>
        public DateTimeOffset LastTimedRespec { get; set; }

        /// <summary>Gets the last medical clone transfer date.</summary>
        public DateTimeOffset RemoteStationDate { get; set; }

        /// <summary>Gets the jump activation expiration date.</summary>
        public DateTimeOffset JumpActivation { get; set; }

        /// <summary>Gets the jump fatigue expiration date.</summary>
        public DateTimeOffset JumpFatigue { get; set; }

        /// <summary>Gets the last jump made by the character.</summary>
        public DateTimeOffset JumpLastUpdate { get; set; }

        /// <summary>Gets the balance.</summary>
        public double Balance { get; set; }

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

        /// <summary>Gets the implants.</summary>
        public IEnumerable<Implant> Implants { get; set; }

        /// <summary>Gets the jump clone implants.</summary>
        public IEnumerable<Implant> JumpCloneImplants { get; set; }
        
        /// <summary>Gets the jump clones.</summary>
        public IEnumerable<JumpClone> JumpClones { get; set; }

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