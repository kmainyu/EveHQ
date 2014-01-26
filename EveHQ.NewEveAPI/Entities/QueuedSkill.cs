// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (QueuedSkill.cs), is part of EveHQ.
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

    /// <summary>The queued skill.</summary>
    public sealed class QueuedSkill
    {
        /// <summary>Gets or sets the queue position.</summary>
        public int QueuePosition { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        public int TypeId { get; set; }

        /// <summary>Gets or sets the level.</summary>
        public int Level { get; set; }

        /// <summary>Gets or sets the start SkillPoints.</summary>
        public int StartSP { get; set; }

        /// <summary>Gets or sets the end SkillPoints.</summary>
        public int EndSP { get; set; }

        /// <summary>Gets or sets the start time.</summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>Gets or sets the end time.</summary>
        public DateTimeOffset EndTime { get; set; }
    }
}