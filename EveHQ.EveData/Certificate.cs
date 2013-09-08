// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Certificate.cs" company="EveHQ Development Team">
//  Copyright © 2005-2012  EveHQ Development Team
// </copyright>
// <summary>
//  Defines an Eve certificate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EveHQ.EveData
{
    using System;
    using System.Collections.Generic;
    using ProtoBuf;

    /// <summary>
    /// Defines an Eve certificate.
    /// </summary>
    [ProtoContract, Serializable]
    public class Certificate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Certificate"/> class.
        /// </summary>
        public Certificate()
        {
            this.RequiredSkills = new SortedList<string, int>();
            this.RequiredCertificates = new SortedList<string, int>();
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        [ProtoMember(2)]
        public int Grade { get; set; }

        /// <summary>
        /// Gets or sets the class ID.
        /// </summary>
        [ProtoMember(3)]
        public int ClassId { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        [ProtoMember(4)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the issuing corp ID.
        /// </summary>
        [ProtoMember(5)]
        public long CorpId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ProtoMember(6)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the pre-requisite skills for the certificate.
        /// </summary>
        [ProtoMember(7)]
        public SortedList<string, int> RequiredSkills { get; set; }

        /// <summary>
        /// Gets or sets the pre-requisite certificates for the certificate.
        /// </summary>
        [ProtoMember(8)]
        public SortedList<string, int> RequiredCertificates { get; set; }
    }
}