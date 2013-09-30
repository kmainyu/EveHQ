// ===========================================================================
// <copyright file="UnifiedDataFormat.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (UnifiedDataFormat.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.Market.UnifiedMarketDataFormat
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

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
        private static readonly UploadGenerator GeneratorName = new UploadGenerator { Name = "EveHQ", Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() };

        #endregion

        #region Fields

        /// <summary>The _json settings.</summary>
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        #endregion

        #region Public Properties

        /// <summary>Gets the current time.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For consistency of the other properties.")]
        public string CurrentTime
        {
            get
            {
                return DateTimeOffset.UtcNow.ToString(Iso8601Format, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets the generator.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For consistency of the other properties.")]
        public UploadGenerator Generator
        {
            get
            {
                return GeneratorName;
            }
        }

        /// <summary>Gets or sets the result type.</summary>
        [JsonConverter(typeof(ResultKindConverter))]
        public ResultType ResultType { get; set; }

        /// <summary>Gets or sets the upload keys.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "needed for json serialization")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "to be refactored later.")]
        public List<UploadKey> UploadKeys { get; set; }

        /// <summary>Gets the version.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For consistency.")]
        public string Version
        {
            get
            {
                return DataVersion;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>The to json.</summary>
        /// <returns>The <see cref="string" />.</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, this.jsonSettings);
        }

        #endregion
    }
}