// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintActivity.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines the activities that can be performed with blueprints.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines the activities that can be performed with blueprints.
    /// </summary>
    [ProtoContract, Serializable]
    public enum BlueprintActivity
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The manufacturing.
        /// </summary>
        Manufacturing = 1,

        /// <summary>
        /// The research tech.
        /// </summary>
        ResearchTech = 2,

        /// <summary>
        /// The research production level.
        /// </summary>
        ResearchProductionLevel = 3,

        /// <summary>
        /// The research material level.
        /// </summary>
        ResearchMaterialLevel = 4,

        /// <summary>
        /// The copying.
        /// </summary>
        Copying = 5,

        /// <summary>
        /// The recycling.
        /// </summary>
        Recycling = 6,

        /// <summary>
        /// The reverse engineering.
        /// </summary>
        ReverseEngineering = 7,

        /// <summary>
        /// The invention.
        /// </summary>
        Invention = 8
    }
}