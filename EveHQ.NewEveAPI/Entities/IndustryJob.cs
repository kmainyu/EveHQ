// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System;

namespace EveHQ.EveApi
{
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
        public int InstalledInSolarSystemId { get; set; }

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