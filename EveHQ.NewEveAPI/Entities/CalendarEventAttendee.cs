﻿// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (CalendarEventAttendee.cs), is part of EveHQ.
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
    /// The calendar event attendee.
    /// </summary>
    public sealed class CalendarEventAttendee
    {
        /// <summary>
        /// Gets the Id of the attendee.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets the Attendee's name.
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// Gets the attendee's response.
        /// </summary>
        public AttendeeResponseType Response { get; set; }
    }
}