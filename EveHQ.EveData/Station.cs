// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Station.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve station.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    ///  Defines an Eve station.
    /// </summary>
    [ProtoContract, Serializable]
    public class Station
    {
        /// <summary>
        /// Gets or sets the station ID.
        /// </summary>
        [ProtoMember(1)]
        public int StationId { get; set; }

        /// <summary>
        /// Gets or sets the station name.
        /// </summary>
        [ProtoMember(2)]
        public string StationName { get; set; }

        /// <summary>
        /// Gets or sets the system ID.
        /// </summary>
        [ProtoMember(3)]
        public int SystemId { get; set; }

        /// <summary>
        /// Gets or sets the corp ID of the station owner.
        /// </summary>
        [ProtoMember(4)]
        public int CorpId { get; set; }

        /// <summary>
        /// Gets or sets the refining efficiency.
        /// </summary>
        [ProtoMember(5)]
        public double RefiningEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the station take when recycling.
        /// </summary>
        [ProtoMember(6)]
        public double StationTake { get; set; }

        /// <summary>
        /// Gets or sets the services the station provides.
        /// </summary>
        [ProtoMember(7)]
        public int Services { get; set; }
    }
}