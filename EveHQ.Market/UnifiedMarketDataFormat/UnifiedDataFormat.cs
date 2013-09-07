// ========================================================================
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
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================
namespace EveHQ.Market.UnifiedMarketDataFormat
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>The Unfied data format for a specific kind of row schema.</summary>
    /// <typeparam name="T"></typeparam>
    public class UnifiedDataFormat<T> : UnifiedDataFormat
    {
        /// <summary>Initializes a new instance of the <see cref="UnifiedDataFormat{T}"/> class.</summary>
        public UnifiedDataFormat()
        {
            Rowsets = new List<T>();
        }

        /// <summary>Gets or sets the columns.</summary>
        public virtual string[] Columns { get; set; }

        /// <summary>Gets or sets the rowsets.</summary>
        public List<T> Rowsets { get; set; }
    }

    /// <summary>The unified data format.</summary>
    public abstract class UnifiedDataFormat
    {
        /// <summary>The data version.</summary>
        private const string DataVersion = "0.1";

        /// <summary>The iso 8601 format.</summary>
        private const string Iso8601Format = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary>The _generator.</summary>
        private static readonly UploadGenerator _generator;

        /// <summary>The _json settings.</summary>
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        /// <summary>Initializes static members of the <see cref="UnifiedDataFormat"/> class.</summary>
        static UnifiedDataFormat()
        {
            _generator = new UploadGenerator { Name = "EveHQ", Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() };
        }

        /// <summary>Gets or sets the result type.</summary>
        [JsonConverter(typeof(ResultKindConverter))]
        public ResultKind ResultType { get; set; }

        /// <summary>Gets the version.</summary>
        public string Version
        {
            get
            {
                return DataVersion;
            }
        }

        /// <summary>Gets or sets the upload keys.</summary>
        public List<UploadKey> UploadKeys { get; set; }

        /// <summary>Gets the generator.</summary>
        public UploadGenerator Generator
        {
            get
            {
                return _generator;
            }
        }

        /// <summary>Gets the current time.</summary>
        public string CurrentTime
        {
            get
            {
                return DateTimeOffset.UtcNow.ToString(Iso8601Format);
            }
        }

        /// <summary>The to json.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, _jsonSettings);
        }
    }
}