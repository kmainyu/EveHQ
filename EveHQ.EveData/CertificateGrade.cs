// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateGrade.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines which grades a certificate can be.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines which grades a certificate can be.
    /// </summary>
    [ProtoContract, Serializable]
    public enum CertificateGrade
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The basic.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// The standard.
        /// </summary>
        Standard = 2,

        /// <summary>
        /// The improved.
        /// </summary>
        Improved = 3,

        /// <summary>
        /// The advanced.
        /// </summary>
        Advanced = 4,

        /// <summary>
        /// The elite.
        /// </summary>
        Elite = 5
    }
}