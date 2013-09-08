// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SolarSystem.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve solar system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using System.Collections.ObjectModel;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve solar system.
    /// </summary>
    [ProtoContract, Serializable]
    public class SolarSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolarSystem"/> class.
        /// </summary>
        public SolarSystem()
        {
            this.Gates = new Collection<int>();
        }

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
        /// Gets or sets the security.
        /// </summary>
        [ProtoMember(3)]
        public double Security { get; set; }

        /// <summary>
        /// Gets or sets the constellation ID.
        /// </summary>
        [ProtoMember(4)]
        public int ConstellationId { get; set; }

        /// <summary>
        /// Gets or sets the region ID.
        /// </summary>
        [ProtoMember(5)]
        public int RegionId { get; set; }

        /// <summary>
        /// Gets or sets the planet count.
        /// </summary>
        [ProtoMember(6)]
        public int PlanetCount { get; set; }

        /// <summary>
        /// Gets or sets the moon count.
        /// </summary>
        [ProtoMember(7)]
        public int MoonCount { get; set; }

        /// <summary>
        /// Gets or sets the station count.
        /// </summary>
        [ProtoMember(8)]
        public int StationCount { get; set; }

        /// <summary>
        /// Gets or sets the ore belt count.
        /// </summary>
        [ProtoMember(9)]
        public int OreBeltCount { get; set; }

        /// <summary>
        /// Gets or sets the ice belt count.
        /// </summary>
        [ProtoMember(10)]
        public int IceBeltCount { get; set; }

        /// <summary>
        /// Gets or sets a list of gates within the system.
        /// </summary>
        [ProtoMember(11)]
        public Collection<int> Gates { get; set; }

        /// <summary>
        /// Gets or sets the x co-ordinate.
        /// </summary>
        [ProtoMember(12)]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y co-ordinate.
        /// </summary>
        [ProtoMember(13)]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the z co-ordinate.
        /// </summary>
        [ProtoMember(14)]
        public double Z { get; set; }
    }
}