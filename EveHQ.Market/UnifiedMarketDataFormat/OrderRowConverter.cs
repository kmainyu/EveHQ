// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (OrderRowConverter.cs), is part of EveHQ.
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
namespace EveHQ.Market
{
    using System;

    using EveHQ.Common.Extensions;

    using Newtonsoft.Json;

    /// <summary>Converts an OrderRow to JSON text.</summary>
    public class OrderRowConverter : JsonConverter
    {
        /// <summary>The can convert.</summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderRow);
        }

        /// <summary>The read json.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The <see cref="object"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
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
            var row = value as OrderRow;
            if (row == null)
            {
                return;
            }

            writer.WriteStartArray();
            writer.WriteRawValue(row.Price.ToInvariantString(2));
            writer.WriteRawValue(row.VolRemaining.ToInvariantString());
            writer.WriteRawValue(row.Range.ToInvariantString());
            writer.WriteRawValue(row.OrderId.ToInvariantString());
            writer.WriteRawValue(row.VolEntered.ToInvariantString());
            writer.WriteRawValue(row.MinVolume.ToInvariantString());
            writer.WriteRawValue(row.Bid.ToInvariantString());
            writer.WriteValue(row.IssueDate);
            writer.WriteRawValue(row.Duration.ToInvariantString());
            writer.WriteRawValue(row.StationId.ToInvariantString());
            writer.WriteRawValue(row.SolarSystemId.ToInvariantString());
            writer.WriteEndArray();

            writer.Flush();
        }
    }
}