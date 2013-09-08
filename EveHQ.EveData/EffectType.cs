// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EffectType.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve effect type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve effect type.
    /// </summary>
    [ProtoContract, Serializable]
    public class EffectType
    {
        /// <summary>
        /// Gets or sets the effect ID.
        /// </summary>
        [ProtoMember(1)]
        public int EffectId { get; set; }

        /// <summary>
        /// Gets or sets the effect name.
        /// </summary>
        [ProtoMember(2)]
        public string EffectName { get; set; }
    }
}