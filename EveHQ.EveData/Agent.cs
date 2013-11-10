// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Agent.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve Agent.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve Agent.
    /// </summary>
    [ProtoContract, Serializable]
    public class Agent
    {
        /// <summary>
        /// Gets or sets the agent id.
        /// </summary>
        [ProtoMember(1)]
        public int AgentId { get; set; }

        /// <summary>
        /// Gets or sets the agent name.
        /// </summary>
        [ProtoMember(2)]
        public string AgentName { get; set; }

        /// <summary>
        /// Gets or sets the division id of the agent.
        /// </summary>
        [ProtoMember(3)]
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the corporation id of the agent.
        /// </summary>
        [ProtoMember(4)]
        public int CorporationId { get; set; }

        /// <summary>
        /// Gets or sets the location ID of the agent.
        /// </summary>
        [ProtoMember(5)]
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the agent level.
        /// </summary>
        [ProtoMember(6)]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the agent type.
        /// </summary>
        [ProtoMember(7)]
        public int AgentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the agent is a locator agent.
        /// </summary>
        [ProtoMember(8)]
        public bool IsLocator { get; set; }
    }
}