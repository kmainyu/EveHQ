// ===========================================================================
// <copyright file="ResultKindConverter.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ResultKindConverter.cs), is part of EveHQ.
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

    using Newtonsoft.Json;

    /// <summary>The result kind converter.</summary>
    public class ResultKindConverter : JsonConverter
    {
        #region Public Methods and Operators

        /// <summary>The can convert.</summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ResultType);
        }

        /// <summary>The read json.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue == null)
            {
                return null;
            }

            return Enum.Parse(typeof(ResultType), existingValue.ToString());
        }

        /// <summary>The write json.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "json and xml normalize to lowercase.")]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                return;
            }

            var kind = (ResultType)value;
            writer.WriteValue(kind.ToString().ToLowerInvariant());
        }

        #endregion
    }
}