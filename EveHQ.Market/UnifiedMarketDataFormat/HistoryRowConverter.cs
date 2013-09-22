// ===========================================================================
// <copyright file="HistoryRowConverter.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (HistoryRowConverter.cs), is part of EveHQ.
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

    using EveHQ.Common.Extensions;

    using Newtonsoft.Json;

    /// <summary>
    ///     Converts a HistoryRow instance into json.
    /// </summary>
    public class HistoryRowConverter : JsonConverter
    {
        #region Public Methods and Operators

        /// <summary>The can convert.</summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HistoryRow);
        }

        /// <summary>The read json.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The <see cref="object"/>.</returns>
        /// <exception cref="NotImplementedException">Not functional.</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>The write json.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                return;
            }

            var row = value as HistoryRow;

            if (row == null)
            {
                return;
            }

            writer.WriteStartArray();
            writer.WriteValue(row.Date);
            writer.WriteValue(row.Orders);
            writer.WriteValue(row.Quantity);
            writer.WriteRawValue(row.Low.ToInvariantString(2));
            writer.WriteRawValue(row.High.ToInvariantString(2));
            writer.WriteRawValue(row.Average.ToInvariantString(2));
            writer.WriteEndArray();
        }

        #endregion
    }
}