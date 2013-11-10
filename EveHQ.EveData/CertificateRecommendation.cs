// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateRecommendation.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines which ships this certificate is recommended for.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines which ships this certificate is recommended for.
    /// </summary>
    [ProtoContract][Serializable]
    public class CertificateRecommendation
    {
        /// <summary>
        /// Gets or sets the ship type ID.
        /// </summary>
        [ProtoMember(1)]
        public int ShipTypeId { get; set; }

        /// <summary>
        /// Gets or sets the certificate ID.
        /// </summary>
        [ProtoMember(2)]
        public int CertificateId { get; set; }
    }
}