// ===========================================================================
// <copyright file="CharacterInfo.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (CharacterInfo.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.EveApi
{
    using System;

    /// <summary>The character info.</summary>
    public class CharacterInfo
    {
        #region Public Properties

        /// <summary>Gets or sets the alliance id.</summary>
        public long AllianceId { get; set; }

        /// <summary>Gets or sets the alliance in date.</summary>
        public DateTimeOffset AllianceInDate { get; set; }

        /// <summary>Gets or sets the alliance name.</summary>
        public string AllianceName { get; set; }

        /// <summary>Gets or sets the bloodline.</summary>
        public string Bloodline { get; set; }

        /// <summary>Gets or sets the character id.</summary>
        public long CharacterId { get; set; }

        /// <summary>Gets or sets the character name.</summary>
        public string CharacterName { get; set; }

        /// <summary>Gets or sets the corporation id.</summary>
        public long CorporationId { get; set; }

        /// <summary>Gets or sets the corporation name.</summary>
        public string CorporationName { get; set; }

        /// <summary>Gets or sets the race.</summary>
        public string Race { get; set; }

        /// <summary>Gets or sets the security status.</summary>
        public double SecurityStatus { get; set; }

        #endregion
    }
}