// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEffect.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve effect assigned to a type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve effect assigned to a type.
    /// </summary>
    [ProtoContract, Serializable]
    public class TypeEffect
    {
        /// <summary>
        /// Gets or sets the type ID.
        /// </summary>
        [ProtoMember(1)]
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the effect ID.
        /// </summary>
        [ProtoMember(2)]
        public int EffectId { get; set; }
    }
}