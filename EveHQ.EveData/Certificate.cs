// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System;
using System.Collections.Generic;
using ProtoBuf;

namespace EveHQ.EveData
{
    /// <summary>
    ///     Defines an Eve certificate.
    /// </summary>
    [ProtoContract, Serializable]
    public class Certificate
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Certificate" /> class.
        /// </summary>
        public Certificate()
        {
            GradesAndSkills = new SortedList<CertificateGrade, SortedList<int, int>>();

            RecommendedTypes = new List<int>();
        }

        /// <summary>
        ///     Gets or sets the ID.
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the group ID.
        /// </summary>
        [ProtoMember(2)]
        public int GroupId { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [ProtoMember(3)]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the name of the certificate
        /// </summary>
        [ProtoMember(4)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the collection of type IDs that benefit from this cert.
        /// </summary>
        [ProtoMember(5)]
        public IList<int> RecommendedTypes { get; set; }

        /// <summary>
        ///     Gets or sets the collection of grades available in this cert and the collection of skills (and their levels) in
        ///     order to qualify.
        /// </summary>
        [ProtoMember(6)]
        public SortedList<CertificateGrade, SortedList<int, int>> GradesAndSkills { get; set; }
    }
}