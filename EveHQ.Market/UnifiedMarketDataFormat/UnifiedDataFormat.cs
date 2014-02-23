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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EveHQ.Market.UnifiedMarketDataFormat
{
    /// <summary>The unified data format.</summary>
    public abstract class UnifiedDataFormat
    {
        #region Constants

        /// <summary>The data version.</summary>
        private const string DataVersion = "0.1";

        /// <summary>The iso 8601 format.</summary>
        private const string Iso8601Format = "yyyy-MM-ddTHH:mm:sszzz";

        #endregion

        #region Static Fields

        /// <summary>The _generator.</summary>
        private static readonly UploadGenerator GeneratorName = new UploadGenerator
        {
            Name = "EveHQ",
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
        };

        #endregion

        #region Fields

        /// <summary>The _json settings.</summary>
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        #endregion

        #region Public Properties

        /// <summary>Gets the current time.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency of the other properties.")]
        public string CurrentTime
        {
            get { return DateTimeOffset.UtcNow.ToString(Iso8601Format, CultureInfo.InvariantCulture); }
        }

        /// <summary>Gets the generator.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency of the other properties.")]
        public UploadGenerator Generator
        {
            get { return GeneratorName; }
        }

        /// <summary>Gets or sets the result type.</summary>
        [JsonConverter(typeof (ResultKindConverter))]
        public ResultType ResultType { get; set; }

        /// <summary>Gets or sets the upload keys.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "needed for json serialization")]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "to be refactored later."
            )]
        public List<UploadKey> UploadKeys { get; set; }

        /// <summary>Gets the version.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For consistency.")]
        public string Version
        {
            get { return DataVersion; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>The to json.</summary>
        /// <returns>The <see cref="string" />.</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, jsonSettings);
        }

        #endregion
    }
}