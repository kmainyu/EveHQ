// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (FactionalWarfareStats.cs), is part of EveHQ.
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

    using Newtonsoft.Json;

    /// <summary>
    /// Factional warfare stats data.
    /// </summary>
    public sealed class FactionalWarfareStats
    {
        /// <summary>Gets the faction id.</summary>
        public int FactionId { get; set; }

        /// <summary>Gets the faction name.</summary>
        public string FactionName { get; set; }

        /// <summary>Gets the enlisted.</summary>
        public DateTimeOffset Enlisted { get; set; }

        /// <summary>Gets the current rank.</summary>
        public int CurrentRank { get; set; }

        /// <summary>Gets the highest rank.</summary>
        public int HighestRank { get; set; }

        /// <summary>Gets the kills yesterday.</summary>
        public int KillsYesterday { get; set; }

        /// <summary>Gets the kills last week.</summary>
        public int KillsLastWeek { get; set; }

        /// <summary>Gets the kills total.</summary>
        public int KillsTotal { get; set; }

        /// <summary>Gets the victory points yesterday.</summary>
        public int VictoryPointsYesterday { get; set; }

        /// <summary>Gets the victory points last week.</summary>
        public int VictoryPointsLastWeek { get; set; }

        /// <summary>Gets the victory points total.</summary>
        public int VictoryPointsTotal { get; set; }
    }
}