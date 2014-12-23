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
        /// <summary>Gets the job ID.</summary>
        public long JobId { get; set; }

        /// <summary>Gets the installer ID.</summary>
        public int InstallerId { get; set; }

        /// <summary>Gets the installer name.</summary>
        public string InstallerName { get; set; }

        /// <summary>Gets the facility ID.</summary>
        public int FacilityId { get; set; }

        /// <summary>Gets the solar system ID.</summary>
        public int SolarSystemId { get; set; }

        /// <summary>Gets the solar system name.</summary>
        public string SolarSystemName { get; set; }

        /// <summary>Gets the station ID.</summary>
        public int StationId { get; set; }
        
        /// <summary>Gets the activity ID.</summary>
        public int ActivityId { get; set; }

        /// <summary>Gets the installed blueprint item ID (from AssetList).</summary>
        public long BlueprintId { get; set; }

        /// <summary>Gets the installed blueprint type ID.</summary>
        public int BlueprintTypeId { get; set; }

        /// <summary>Gets the installed blueprint type name.</summary>
        public string BlueprintTypeName { get; set; }

        /// <summary>Gets the blueprint location id.</summary>
        public long BlueprintLocationId { get; set; }
        
        /// <summary>Gets the output location id.</summary>
        public long OutputLocationId { get; set; }

        /// <summary>Gets the runs.</summary>
        public int Runs { get; set; }

        /// <summary>Gets the cost.</summary>
        public double Cost { get; set; }

        /// <summary>Gets the team ID.</summary>
        public int TeamId { get; set; }

        /// <summary>Gets the licensed runs.</summary>
        public int LicensedRuns { get; set; }

        /// <summary>Gets the probability.</summary>
        public double Probability { get; set; }

        /// <summary>Gets the product type ID.</summary>
        public int ProductTypeId { get; set; }

        /// <summary>Gets the product type name.</summary>
        public string ProductTypeName { get; set; }

        /// <summary>Gets the status.</summary>
        public IndustryJobStatus Status { get; set; }

        /// <summary>Gets the duration of the job.</summary>
        public int TimeInSeconds { get; set; }

        /// <summary>Gets the start date.</summary>
        public DateTimeOffset StartDate { get; set; }
        
        /// <summary>Gets the end date.</summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>Gets the pause date.</summary>
        public DateTimeOffset PauseDate { get; set; }

        /// <summary>Gets the completed date.</summary>
        public DateTimeOffset CompletedDate { get; set; }

        /// <summary>Gets the completed character ID.</summary>
        public int CompletedCharacterId { get; set; }
    }
}