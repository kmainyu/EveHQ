// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketGroup.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve market group.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve market group.
    /// </summary>
    [ProtoContract, Serializable]
    public class MarketGroup
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
        /// Gets or sets the parent group ID.
        /// </summary>
        [ProtoMember(3)]
        public int ParentGroupId { get; set; }
    }
}