﻿// ==============================================================================
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
using ProtoBuf;

namespace EveHQ.EveData
{
    /// <summary>
    ///     Defines an Eve item/type.
    /// </summary>
    [ProtoContract, Serializable]
    public class EveType
    {
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
        ///     Gets or sets the description.
        /// </summary>
        [ProtoMember(3)]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        [ProtoMember(4)]
        public int Group { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        [ProtoMember(5)]
        public int Category { get; set; }

        /// <summary>
        ///     Gets or sets the market group ID.
        /// </summary>
        [ProtoMember(6)]
        public int MarketGroupId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the type is published.
        /// </summary>
        [ProtoMember(7)]
        public bool Published { get; set; }

        /// <summary>
        ///     Gets or sets the mass.
        /// </summary>
        [ProtoMember(8)]
        public double Mass { get; set; }

        /// <summary>
        ///     Gets or sets the capacity.
        /// </summary>
        [ProtoMember(9)]
        public double Capacity { get; set; }

        /// <summary>
        ///     Gets or sets the volume.
        /// </summary>
        [ProtoMember(10)]
        public double Volume { get; set; }

        /// <summary>
        ///     Gets or sets the meta level.
        /// </summary>
        [ProtoMember(11)]
        public int MetaLevel { get; set; }

        /// <summary>
        ///     Gets or sets the portion size.
        /// </summary>
        [ProtoMember(12)]
        public int PortionSize { get; set; }

        /// <summary>
        ///     Gets or sets the base price.
        /// </summary>
        [ProtoMember(13)]
        public double BasePrice { get; set; }
    }
}