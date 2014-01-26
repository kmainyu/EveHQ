// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (UpcomingCalendarEvent.cs), is part of EveHQ.
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

    /// <summary>The upcoming calendar event.</summary>
    public class UpcomingCalendarEvent
    {
        /// <summary>Gets or sets the event id.</summary>
        public long EventId { get; set; }

        /// <summary>Gets or sets the owner id.</summary>
        public long OwnerId { get; set; }

        /// <summary>Gets or sets the owner name.</summary>
        public string OwnerName { get; set; }

        /// <summary>Gets or sets the event date.</summary>
        public DateTimeOffset EventDate { get; set; }

        /// <summary>Gets or sets the event title.</summary>
        public string EventTitle { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Gets or sets a value indicating whether is important.</summary>
        public bool IsImportant { get; set; }

        /// <summary>Gets or sets the event text.</summary>
        public string EventText { get; set; }

        /// <summary>Gets or sets the response.</summary>
        public string Response { get; set; }
    }
}