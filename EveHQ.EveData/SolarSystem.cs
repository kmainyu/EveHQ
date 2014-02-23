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
using System.Collections.ObjectModel;
using ProtoBuf;

namespace EveHQ.EveData
{
    /// <summary>
    ///     Defines an Eve solar system.
    /// </summary>
    [ProtoContract, Serializable]
    public class SolarSystem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SolarSystem" /> class.
        /// </summary>
        public SolarSystem()
        {
            Gates = new Collection<int>();
        }

        /// <summary>
        ///     Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the security.
        /// </summary>
        [ProtoMember(3)]
        public double Security { get; set; }

        /// <summary>
        ///     Gets or sets the constellation ID.
        /// </summary>
        [ProtoMember(4)]
        public int ConstellationId { get; set; }

        /// <summary>
        ///     Gets or sets the region ID.
        /// </summary>
        [ProtoMember(5)]
        public int RegionId { get; set; }

        /// <summary>
        ///     Gets or sets the planet count.
        /// </summary>
        [ProtoMember(6)]
        public int PlanetCount { get; set; }

        /// <summary>
        ///     Gets or sets the moon count.
        /// </summary>
        [ProtoMember(7)]
        public int MoonCount { get; set; }

        /// <summary>
        ///     Gets or sets the station count.
        /// </summary>
        [ProtoMember(8)]
        public int StationCount { get; set; }

        /// <summary>
        ///     Gets or sets the ore belt count.
        /// </summary>
        [ProtoMember(9)]
        public int OreBeltCount { get; set; }

        /// <summary>
        ///     Gets or sets the ice belt count.
        /// </summary>
        [ProtoMember(10)]
        public int IceBeltCount { get; set; }

        /// <summary>
        ///     Gets or sets a list of gates within the system.
        /// </summary>
        [ProtoMember(11)]
        public Collection<int> Gates { get; set; }

        /// <summary>
        ///     Gets or sets the x co-ordinate.
        /// </summary>
        [ProtoMember(12)]
        public double X { get; set; }

        /// <summary>
        ///     Gets or sets the y co-ordinate.
        /// </summary>
        [ProtoMember(13)]
        public double Y { get; set; }

        /// <summary>
        ///     Gets or sets the z co-ordinate.
        /// </summary>
        [ProtoMember(14)]
        public double Z { get; set; }
    }
}