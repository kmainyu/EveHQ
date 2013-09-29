// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (Notification.cs), is part of EveHQ.
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

    /// <summary>
    /// A single notification message.
    /// </summary>
    public sealed class Notification
    {
        /// <summary>Gets or sets the notification id.</summary>
        public int NotificationId { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        public NotificationType TypeId { get; set; }

        /// <summary>Gets or sets the sender id.</summary>
        public int SenderId { get; set; }

        /// <summary>Gets or sets the sent date.</summary>
        public DateTimeOffset SentDate { get; set; }

        /// <summary>Gets or sets a value indicating whether is read.</summary>
        public bool IsRead { get; set; }
    }
}