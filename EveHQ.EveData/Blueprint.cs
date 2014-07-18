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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProtoBuf;

namespace EveHQ.EveData
{
    /// <summary>
    ///     Defines an Eve blueprint.
    /// </summary>
    [ProtoContract, Serializable]
    public class Blueprint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Blueprint" /> class.
        /// </summary>
        public Blueprint()
        {
            Resources = new Dictionary<int, Dictionary<int, BlueprintResource>>();
            Inventions = new Collection<int>();
            InventionMetaItems = new Collection<int>();
            InventFrom = new Collection<int>();
        }

        /// <summary>
        ///     Gets or sets the type ID of the blueprint.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the asset ID.
        /// </summary>
        [ProtoMember(2)]
        public long AssetId { get; set; }

        /// <summary>
        ///     Gets or sets the product ID produced by the blueprint.
        /// </summary>
        [ProtoMember(3)]
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the tech level.
        /// </summary>
        [ProtoMember(4)]
        public int TechLevel { get; set; }

        /// <summary>
        ///     Gets or sets the waste factor.
        /// </summary>
        [ProtoMember(5)]
        public int WasteFactor { get; set; }

        /// <summary>
        ///     Gets or sets the material modifier.
        /// </summary>
        [ProtoMember(6)]
        public int MaterialModifier { get; set; }

        /// <summary>
        ///     Gets or sets the productivity modifier.
        /// </summary>
        [ProtoMember(7)]
        public int ProductivityModifier { get; set; }

        /// <summary>
        ///     Gets or sets the max production limit.
        /// </summary>
        [ProtoMember(8)]
        public int MaxProductionLimit { get; set; }

        /// <summary>
        ///     Gets or sets the production time.
        /// </summary>
        [ProtoMember(9)]
        public long ProductionTime { get; set; }

        /// <summary>
        ///     Gets or sets the research material level time.
        /// </summary>
        [ProtoMember(10)]
        public long ResearchMaterialLevelTime { get; set; }

        /// <summary>
        ///     Gets or sets the research production level time.
        /// </summary>
        [ProtoMember(11)]
        public long ResearchProductionLevelTime { get; set; }

        /// <summary>
        ///     Gets or sets the research copy time.
        /// </summary>
        [ProtoMember(12)]
        public long ResearchCopyTime { get; set; }

        /// <summary>
        ///     Gets or sets the research tech time.
        /// </summary>
        [ProtoMember(13)]
        public long ResearchTechTime { get; set; }

        /// <summary>
        ///     Gets or sets the resources that this blueprint requires.
        /// </summary>
        [ProtoMember(14)]
        public Dictionary<int, Dictionary<int, BlueprintResource>> Resources { get; set; }

        /// <summary>
        ///     Gets or sets the item IDs that can be invented from this blueprint.
        /// </summary>
        [ProtoMember(15)]
        public Collection<int> Inventions { get; set; }

        /// <summary>
        ///     Gets or sets the invention meta items.
        /// </summary>
        [ProtoMember(16)]
        public Collection<int> InventionMetaItems { get; set; }

        /// <summary>
        ///     Gets or sets the items IDs that this blueprint can be invented from.
        /// </summary>
        [ProtoMember(17)]
        public Collection<int> InventFrom { get; set; }

        /// <summary>
        /// Gets or sets the quantity produced in 1 run
        /// </summary>
        [ProtoMember(18)]
        public int ProductQuantity { get; set; }
    }
}