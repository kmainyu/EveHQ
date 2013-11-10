// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaType.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve meta type relationship.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve meta type relationship.
    /// </summary>
    [ProtoContract, Serializable]
    public class MetaType
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        [ProtoMember(2)]
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the meta group ID.
        /// </summary>
        [ProtoMember(3)]
        public int MetaGroupId { get; set; }
    }
}