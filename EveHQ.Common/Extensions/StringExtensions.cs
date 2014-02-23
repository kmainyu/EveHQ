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
using System.Globalization;

namespace EveHQ.Common.Extensions
{
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
        /// <returns>The <see cref="bool" />.</returns>
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
        /// <returns>The <see cref="DateTimeOffset" />.</returns>
        public static DateTimeOffset ToDateTimeOffset(this string word, int? forcedOffset)
        {
            DateTimeOffset result;
            DateTime temp;
            if (DateTime.TryParse(word, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp) &&
                forcedOffset.HasValue)
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
        /// <returns>The <see cref="long" />.</returns>
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
            if (Enum.TryParse(word, out value))
            {
                return value;
            }

            return default(T);
        }

        #endregion
    }
}