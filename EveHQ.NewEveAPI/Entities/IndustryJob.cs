// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (IndustryJob.cs), is part of EveHQ.
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

    /// <summary>The industry job.</summary>
    public sealed class IndustryJob
    {
        /// <summary>Gets the job id.</summary>
        public long JobId { get; set; }

        /// <summary>Gets the assembly line id.</summary>
        public long AssemblyLineId { get; set; }

        /// <summary>Gets the container id.</summary>
        public long ContainerId { get; set; }

        /// <summary>Gets the installed item id.</summary>
        public long InstalledItemId { get; set; }

        /// <summary>Gets the installed item location id.</summary>
        public long InstalledItemLocationId { get; set; }

        /// <summary>Gets the installed item quantity.</summary>
        public int InstalledItemQuantity { get; set; }

        /// <summary>Gets the installed item productivity level.</summary>
        public int InstalledItemProductivityLevel { get; set; }

        /// <summary>Gets the installed item material level.</summary>
        public int InstalledItemMaterialLevel { get; set; }

        /// <summary>Gets the installed item licensed production runs remaining.</summary>
        public int InstalledItemLicensedProductionRunsRemaining { get; set; }

        /// <summary>Gets the output location id.</summary>
        public long OutputLocationId { get; set; }

        /// <summary>Gets the installer id.</summary>
        public long InstallerId { get; set; }

        /// <summary>Gets the runs.</summary>
        public int Runs { get; set; }

        /// <summary>Gets the licensed production runs.</summary>
        public int LicensedProductionRuns { get; set; }

        /// <summary>Gets the installed in solar system id.</summary>
        public long InstalledInSolarSystemId { get; set; }

        /// <summary>Gets the container location id.</summary>
        public long ContainerLocationId { get; set; }

        /// <summary>Gets the material multiplier.</summary>
        public double MaterialMultiplier { get; set; }

        /// <summary>Gets the char material multiplier.</summary>
        public double CharMaterialMultiplier { get; set; }

        /// <summary>Gets the time multiplier.</summary>
        public double TimeMultiplier { get; set; }

        /// <summary>Gets the char time multiplier.</summary>
        public double CharTimeMultiplier { get; set; }

        /// <summary>Gets the installed item type id.</summary>
        public int InstalledItemTypeId { get; set; }

        /// <summary>Gets the output type id.</summary>
        public int OutputTypeId { get; set; }

        /// <summary>Gets the container type id.</summary>
        public long ContainerTypeId { get; set; }

        /// <summary>Gets a value indicating whether installed item copy.</summary>
        public bool InstalledItemCopy { get; set; }

        /// <summary>Gets a value indicating whether completed.</summary>
        public bool Completed { get; set; }

        /// <summary>Gets a value indicating whether completed successfully.</summary>
        public bool CompletedSuccessfully { get; set; }

        /// <summary>Gets a value indicating whether installed item flag.</summary>
        public bool InstalledItemFlag { get; set; }

        /// <summary>Gets the output flag.</summary>
        public int OutputFlag { get; set; }

        /// <summary>Gets the activity id.</summary>
        public int ActivityId { get; set; }

        /// <summary>Gets the completed status.</summary>
        public IndustryJobCompletedStatus CompletedStatus { get; set; }

        /// <summary>Gets the install time.</summary>
        public DateTimeOffset InstallTime { get; set; }

        /// <summary>Gets the begin production time.</summary>
        public DateTimeOffset BeginProductionTime { get; set; }

        /// <summary>Gets the end production time.</summary>
        public DateTimeOffset EndProductionTime { get; set; }

        /// <summary>Gets the pause production time.</summary>
        public DateTimeOffset PauseProductionTime { get; set; }
    }
}