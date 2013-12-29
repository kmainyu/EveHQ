// ===========================================================================
// <copyright file="ConquerableStation.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (ConquerableStation.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
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
    /// <summary>The conquerable station.</summary>
    public class ConquerableStation
    {
        /// <summary>Gets or sets the id.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the station type id.</summary>
        public int StationTypeId { get; set; }

        /// <summary>Gets or sets the solar system id.</summary>
        public int SolarSystemId { get; set; }

        /// <summary>Gets or sets the corporation id.</summary>
        public int CorporationId { get; set; }

        /// <summary>Gets or sets the corporation name.</summary>
        public string CorporationName { get; set; }
    }
}