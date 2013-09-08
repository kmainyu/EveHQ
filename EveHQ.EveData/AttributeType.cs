// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeType.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve attribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve attribute type.
    /// </summary>
    [ProtoContract, Serializable]
    public class AttributeType
    {
        /// <summary>
        /// Gets or sets the attribute ID.
        /// </summary>
        [ProtoMember(1)]
        public int AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        [ProtoMember(2)]
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the display name of the attribute.
        /// </summary>
        [ProtoMember(3)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unit id of the attribute.
        /// </summary>
        [ProtoMember(4)]
        public int UnitId { get; set; }

        /// <summary>
        /// Gets or sets the category id of the attribute.
        /// </summary>
        [ProtoMember(5)]
        public int CategoryId { get; set; }
    }
}