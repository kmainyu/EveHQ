// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeAttrib.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve attribute assigned to a type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve attribute assigned to a type.
    /// </summary>
    [ProtoContract, Serializable]
    public class TypeAttrib
    {
        /// <summary>
        /// Gets or sets the type ID.
        /// </summary>
        [ProtoMember(1)]
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute ID.
        /// </summary>
        [ProtoMember(2)]
        public int AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute value.
        /// </summary>
        [ProtoMember(3)]
        public double Value { get; set; }
    }
}