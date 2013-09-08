// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateCategory.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines an Eve certificate category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve certificate category.
    /// </summary>
    [ProtoContract, Serializable]
    public class CertificateCategory
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
    }
}