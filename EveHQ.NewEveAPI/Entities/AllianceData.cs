// ===========================================================================
// <copyright file="AllianceData.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (AllianceData.cs), is part of EveHQ.
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
    using System;
    using System.Collections.Generic;

    /// <summary>The alliance data.</summary>
    public class AllianceData
    {
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the short name.</summary>
        public string ShortName { get; set; }

        /// <summary>Gets or sets the id.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the executor corp id.</summary>
        public int ExecutorCorpId { get; set; }

        /// <summary>Gets or sets the member count.</summary>
        public int MemberCount { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>Gets or sets the member corps.</summary>
        public IEnumerable<AllianceCorpData> MemberCorps { get; set; }
    }

    /// <summary>The alliance corp data.</summary>
    public class AllianceCorpData
    {
        /// <summary>Gets or sets the corporation id.</summary>
        public int CorporationId { get; set; }

        /// <summary>Gets or sets the joined date.</summary>
        public DateTimeOffset JoinedDate { get; set; }
    }
}