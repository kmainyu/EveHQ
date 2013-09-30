// ===========================================================================
// <copyright file="ObjectExtensions.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ObjectExtensions.cs), is part of EveHQ.
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
    ///     Conversion methods for objects that are really numbers.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Public Methods and Operators

        /// <summary>The to boolean.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ToBoolean(this object value)
        {
            return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        /// <summary>The to double.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        /// <summary>The to int.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        /// <summary>The to long.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public static long ToInt64(this object value)
        {
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}