// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (SkillInTraining.cs), is part of EveHQ.
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

    /// <summary>The skill in training.</summary>
    public sealed class SkillInTraining
    {
        /// <summary>Gets or sets the current Tranquility time.</summary>
        public DateTimeOffset CurrentTQTime { get; set; }

        /// <summary>Gets or sets the training end time.</summary>
        public DateTimeOffset TrainingEndTime { get; set; }

        /// <summary>Gets or sets the training start time.</summary>
        public DateTimeOffset TrainingStartTime { get; set; }

        /// <summary>Gets or sets the training type id.</summary>
        public int TrainingTypeId { get; set; }

        /// <summary>Gets or sets the training start SkillPoints.</summary>
        public int TrainingStartSP { get; set; }

        /// <summary>Gets or sets the training end SkillPoints.</summary>
        public int TrainingEndSP { get; set; }

        /// <summary>Gets or sets the training to level.</summary>
        public int TrainingToLevel { get; set; }

        /// <summary>Gets or sets a value indicating whether is training.</summary>
        public bool IsTraining { get; set; }
    }
}