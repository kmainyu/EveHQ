// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (NumericalExtensions.cs), is part of EveHQ.
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
    using System.Globalization;

    /// <summary>
    /// The numerical extensions.
    /// </summary>
    public static class NumericalExtensions
    {
        /// <summary>
        /// The to invariant string.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToInvariantString(this int number)
        {
            return number.ToInvariantString(null);
        }

        public static string ToInvariantString(this int number, string formatter)
        {
            return number.ToString(formatter,CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this long number)
        {
            return number.ToInvariantString(null);
        }

        public static string ToInvariantString(this long number, string formatter)
        {
            return number.ToString(formatter,CultureInfo.InvariantCulture);
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="boolean values should be serialized to lower for use in xml or json.")]
        public static string ToInvariantString(this bool value)
        {
            return value.ToString().ToLower(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this double number,int decimalPlaces)
        {
            const string Formatter = "F{0}";

            return number.ToString(Formatter.FormatInvariant(decimalPlaces),CultureInfo.InvariantCulture);
        }


        public static string ToInvariantString(this double number, string formatter)
        {
            return number.ToString(formatter, CultureInfo.InvariantCulture);
        }


       

       
    }
}