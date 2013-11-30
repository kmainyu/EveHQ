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
            GradesAndSkills = new SortedList<CertificateGrade, SortedList<int, int>>();

            RecommendedTypes = new List<int>();
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the group ID.
        /// </summary>
        [ProtoMember(2)]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ProtoMember(3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the certificate
        /// </summary>
        [ProtoMember(4)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of type IDs that benefit from this cert.
        /// </summary>
        [ProtoMember(5)]
        public IList<int> RecommendedTypes { get; set; }

        /// <summary>
        /// Gets or sets the collection of grades available in this cert and the collection of skills (and their levels) in order to qualify.
        /// </summary>
        [ProtoMember(6)]
        public SortedList<CertificateGrade, SortedList<int, int>> GradesAndSkills { get; set; }
    }
}