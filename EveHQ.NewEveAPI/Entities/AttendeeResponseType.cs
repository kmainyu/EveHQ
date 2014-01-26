// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (AttendeeResponseType.cs), is part of EveHQ.
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
    /// <summary>
    /// The attendee response type.
    /// </summary>
    public enum AttendeeResponseType
    {
        /// <summary>
        /// Error state
        /// </summary>
        Invalid,

        /// <summary>
        /// Attendee hasn't taken action on this request.
        /// </summary>
        Undecided,

        /// <summary>
        /// They accepted.
        /// </summary>
        Accepted,

        /// <summary>
        /// Declined
        /// </summary>
        Declined,

        /// <summary>
        /// Attendee might be there.
        /// </summary>
        Tentative
    }
}