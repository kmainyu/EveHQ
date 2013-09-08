// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintResource.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines an individual resource used by a blueprint.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an individual resource used by a blueprint.
    /// </summary>
    [ProtoContract, Serializable]
    public class BlueprintResource
    {
        /// <summary>
        /// Gets or sets the type id.
        /// </summary>
        [ProtoMember(1)]
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the type group.
        /// </summary>
        [ProtoMember(2)]
        public int TypeGroup { get; set; }

        /// <summary>
        /// Gets or sets the type category.
        /// </summary>
        [ProtoMember(3)]
        public int TypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        [ProtoMember(4)]
        public int Activity { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [ProtoMember(5)]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the damage per job.
        /// </summary>
        [ProtoMember(6)]
        public double DamagePerJob { get; set; }

        /// <summary>
        /// Gets or sets the base material.
        /// </summary>
        [ProtoMember(7)]
        public int BaseMaterial { get; set; }
    }
}