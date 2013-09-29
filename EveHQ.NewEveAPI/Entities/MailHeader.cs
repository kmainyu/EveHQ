// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (MailHeader.cs), is part of EveHQ.
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

    /// <summary>The mail header.</summary>
    public sealed class MailHeader
    {
        /// <summary>Gets the message id.</summary>
        public int MessageId { get; set; }

        /// <summary>Gets the sender id.</summary>
        public int SenderId { get; set; }

        /// <summary>Gets the sent date.</summary>
        public DateTimeOffset SentDate { get; set; }

        /// <summary>Gets the title.</summary>
        public string Title { get; set; }

        /// <summary>Gets the to corp or alliance id.</summary>
        public string ToCorpOrAllianceId { get; set; }

        /// <summary>Gets the to character ids.</summary>
        public IEnumerable<int> ToCharacterIds { get; set; }

        /// <summary>Gets the to list list ids.</summary>
        public IEnumerable<int> ToListListIds { get; set; }
    }
}