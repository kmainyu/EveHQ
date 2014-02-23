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
    /// <summary>
    ///     Factional warfare stats data.
    /// </summary>
    public sealed class FactionalWarfareStats
    {
        /// <summary>Gets the faction id.</summary>
        public int FactionId { get; set; }

        /// <summary>Gets the faction name.</summary>
        public string FactionName { get; set; }

        /// <summary>Gets the enlisted.</summary>
        public DateTimeOffset Enlisted { get; set; }

        /// <summary>Gets the current rank.</summary>
        public int CurrentRank { get; set; }

        /// <summary>Gets the highest rank.</summary>
        public int HighestRank { get; set; }

        /// <summary>Gets the kills yesterday.</summary>
        public int KillsYesterday { get; set; }

        /// <summary>Gets the kills last week.</summary>
        public int KillsLastWeek { get; set; }

        /// <summary>Gets the kills total.</summary>
        public int KillsTotal { get; set; }

        /// <summary>Gets the victory points yesterday.</summary>
        public int VictoryPointsYesterday { get; set; }

        /// <summary>Gets the victory points last week.</summary>
        public int VictoryPointsLastWeek { get; set; }

        /// <summary>Gets the victory points total.</summary>
        public int VictoryPointsTotal { get; set; }
    }
}