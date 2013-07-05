// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (StringExtensions.cs), is part of EveHQ.
// 
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
// 
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================

namespace EveHQ.Common.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// String Utility methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Formats the string using the invariant culture</summary>
        /// <param name="format">string to format.</param>
        /// <param name="formatParams">parameters to format into the string.</param>
        /// <returns>A new string instance.</returns>
        public static string FormatInvariant(this string format, params object[] formatParams)
        {
            return string.Format(CultureInfo.InvariantCulture, format, formatParams);
        }

        /// <summary>Checks if the string is null, empty or full of whitespace.</summary>
        /// <param name="word">String to check</param>
        /// <returns>True or False</returns>
        public static bool IsNullOrWhiteSpace(this string word)
        {
            return string.IsNullOrEmpty(word) || word.All(char.IsWhiteSpace);
        }

        public static long ToLong(this string word)
        {
            long result;
            long.TryParse(word, out result);
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
            if (forcedOffset.HasValue && DateTime.TryParse(word,CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                result = new DateTimeOffset(temp, TimeSpan.FromHours(forcedOffset.Value));
            }
            else
            {
                DateTimeOffset.TryParse(word, out result);
            }

            return result;
        }

        /// <summary>Converts the string to a 32bit integer.</summary>
        /// <param name="word">The word.</param>
        /// <returns>The numerical value. Defaults to 0 if not a valid number.</returns>
        public static int ToInt(this string word)
        {
            int result;
            int.TryParse(word, NumberStyles.Any,CultureInfo.InvariantCulture, out result);
            return result;
        }

        /// <summary>
        /// Converts a string number into a double value
        /// </summary>
        /// <param name="word">the string to process</param>
        /// <returns>a double value</returns>
        public static double ToDouble(this string word)
        {
            double result;
            double.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static bool ToBool(this string word)
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
    }
}