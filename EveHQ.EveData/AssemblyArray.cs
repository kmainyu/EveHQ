// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyArray.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//   Defines an Eve assembly array.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using System.Collections.ObjectModel;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve assembly array.
    /// </summary>
    [ProtoContract, Serializable]
    public class AssemblyArray
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyArray"/> class.
        /// </summary>
        public AssemblyArray()
        {
            this.AllowableGroups = new Collection<int>();
            this.AllowableCategories = new Collection<int>();
        }

        /// <summary>
        /// Gets or sets the ID of the assembly array.
        /// </summary>
        [ProtoMember(1)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly array.
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time multiplier.
        /// </summary>
        [ProtoMember(3)]
        public double TimeMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the material multiplier.
        /// </summary>
        [ProtoMember(4)]
        public double MaterialMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the allowable groups.
        /// </summary>
        [ProtoMember(5)]
        public Collection<int> AllowableGroups { get; set; }
        
        /// <summary>
        /// Gets or sets the allowable categories.
        /// </summary>
        [ProtoMember(6)]
        public Collection<int> AllowableCategories { get; set; }
    }
}