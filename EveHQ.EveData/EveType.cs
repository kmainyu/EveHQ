// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EveType.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve item/type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve item/type.
    /// </summary>
    [ProtoContract, Serializable]
    public class EveType
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ProtoMember(3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        [ProtoMember(4)]
        public int Group { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        [ProtoMember(5)]
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the market group ID.
        /// </summary>
        [ProtoMember(6)]
        public int MarketGroupId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is published.
        /// </summary>
        [ProtoMember(7)]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        [ProtoMember(8)]
        public double Mass { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        [ProtoMember(9)]
        public double Capacity { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        [ProtoMember(10)]
        public double Volume { get; set; }

        /// <summary>
        /// Gets or sets the meta level.
        /// </summary>
        [ProtoMember(11)]
        public int MetaLevel { get; set; }

        /// <summary>
        /// Gets or sets the portion size.
        /// </summary>
        [ProtoMember(12)]
        public int PortionSize { get; set; }

        /// <summary>
        /// Gets or sets the base price.
        /// </summary>
        [ProtoMember(13)]
        public double BasePrice { get; set; }
    }
}