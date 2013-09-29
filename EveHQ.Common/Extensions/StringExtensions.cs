// ===========================================================================
// <copyright file="StringExtensions.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (StringExtensions.cs), is part of EveHQ.
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
namespace EveHQ.Common.Extensions
{
    using System;
    using System.Globalization;

    /// <summary>
    ///     String Utility methods
    /// </summary>
    public static class StringExtensions
    {
        #region Public Methods and Operators

        /// <summary>Formats the string using the invariant culture</summary>
        /// <param name="format">string to format.</param>
        /// <param name="parameters">parameters to format into the string.</param>
        /// <returns>A new string instance.</returns>
        public static string FormatInvariant(this string format, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, format, parameters);
        }

        /// <summary>Checks if the string is null, empty or full of whitespace.</summary>
        /// <param name="word">String to check</param>
        /// <returns>True or False</returns>
        public static bool IsNullOrWhiteSpace(this string word)
        {
            return string.IsNullOrWhiteSpace(word);
        }

        /// <summary>The to boolean.</summary>
        /// <param name="word">The word.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ToBoolean(this string word)
        {
            bool result;
            if (!bool.TryParse(word, out result))
            {
                // try numerical processing
                int temp;
                if (int.TryParse(word, out temp) && temp == 1)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>The to date time offset.</summary>
        /// <param name="word">The word.</param>
        /// <param name="forcedOffset">The forced offset.</param>
        /// <returns>The <see cref="DateTimeOffset"/>.</returns>
        public static DateTimeOffset ToDateTimeOffset(this string word, int? forcedOffset)
        {
            DateTimeOffset result;
            DateTime temp;
            if (DateTime.TryParse(word, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp) && forcedOffset.HasValue)
            {
                result = new DateTimeOffset(temp, TimeSpan.FromHours(forcedOffset.Value));
            }
            else
            {
                if (!DateTimeOffset.TryParse(word, out result))
                {
                    return default(DateTimeOffset);
                }
            }

            return result;
        }

        /// <summary>Converts a string number into a double value</summary>
        /// <param name="word">the string to process</param>
        /// <returns>a double value</returns>
        public static double ToDouble(this string word)
        {
            double result;

            if (double.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>Converts the string to a 32bit integer.</summary>
        /// <param name="word">The word.</param>
        /// <returns>The numerical value. Defaults to 0 if not a valid number.</returns>
        public static int ToInt32(this string word)
        {
            int result;
            if (int.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>The to int 64.</summary>
        /// <param name="word">The word.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public static long ToInt64(this string word)
        {
            long result;
            if (long.TryParse(word, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>Converts the string into an Enumerator Type.</summary>
        /// <param name="word">The word.</param>
        /// <typeparam name="T">type to convert to</typeparam>
        /// <returns>An instance of the type</returns>
        public static T ToEnum<T>(this string word) where T : struct
        {
            T value;
            Enum.TryParse(word, out value);

            return value;
        }

        #endregion
    }
}