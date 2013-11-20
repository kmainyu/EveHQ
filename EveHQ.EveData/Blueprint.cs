// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Blueprint.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve blueprint.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve blueprint.
    /// </summary>
    [ProtoContract, Serializable]
    public class Blueprint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blueprint"/> class.
        /// </summary>
        public Blueprint()
        {
            this.Resources = new Dictionary<int, Dictionary<int, BlueprintResource>>();
            this.Inventions = new Collection<int>();
            this.InventionMetaItems = new Collection<int>();
            this.InventFrom = new Collection<int>();
        }

        /// <summary>
        /// Gets or sets the type ID of the blueprint.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the asset ID.
        /// </summary>
        [ProtoMember(2)]
        public long AssetId { get; set; }

        /// <summary>
        /// Gets or sets the product ID produced by the blueprint.
        /// </summary>
        [ProtoMember(3)]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the tech level.
        /// </summary>
        [ProtoMember(4)]
        public int TechLevel { get; set; }

        /// <summary>
        /// Gets or sets the waste factor.
        /// </summary>
        [ProtoMember(5)]
        public int WasteFactor { get; set; }

        /// <summary>
        /// Gets or sets the material modifier.
        /// </summary>
        [ProtoMember(6)]
        public int MaterialModifier { get; set; }

        /// <summary>
        /// Gets or sets the productivity modifier.
        /// </summary>
        [ProtoMember(7)]
        public int ProductivityModifier { get; set; }

        /// <summary>
        /// Gets or sets the max production limit.
        /// </summary>
        [ProtoMember(8)]
        public int MaxProductionLimit { get; set; }

        /// <summary>
        /// Gets or sets the production time.
        /// </summary>
        [ProtoMember(9)]
        public long ProductionTime { get; set; }

        /// <summary>
        /// Gets or sets the research material level time.
        /// </summary>
        [ProtoMember(10)]
        public long ResearchMaterialLevelTime { get; set; }

        /// <summary>
        /// Gets or sets the research production level time.
        /// </summary>
        [ProtoMember(11)]
        public long ResearchProductionLevelTime { get; set; }

        /// <summary>
        /// Gets or sets the research copy time.
        /// </summary>
        [ProtoMember(12)]
        public long ResearchCopyTime { get; set; }

        /// <summary>
        /// Gets or sets the research tech time.
        /// </summary>
        [ProtoMember(13)]
        public long ResearchTechTime { get; set; }

        /// <summary>
        /// Gets or sets the resources that this blueprint requires.
        /// </summary>
        [ProtoMember(14)]
        public Dictionary<int, Dictionary<int, BlueprintResource>> Resources { get; set; }

        /// <summary>
        /// Gets or sets the item IDs that can be invented from this blueprint.
        /// </summary>
        [ProtoMember(15)]
        public Collection<int> Inventions { get; set; }

        /// <summary>
        /// Gets or sets the invention meta items.
        /// </summary>
        [ProtoMember(16)]
        public Collection<int> InventionMetaItems { get; set; }

        /// <summary>
        /// Gets or sets the items IDs that this blueprint can be invented from.
        /// </summary>
        [ProtoMember(17)]
        public Collection<int> InventFrom { get; set; }
    }
}