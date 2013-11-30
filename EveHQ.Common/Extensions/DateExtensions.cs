// ===========================================================================
// <copyright file="DateExtensions.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (DateExtensions.cs), is part of EveHQ.
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
    
    /// <summary>
    ///     Date Utility methods
    /// </summary>
    public static class DateExtensions
    {
        #region Public Methods and Operators

        /// <summary>Converts a date that is in Eve Time to the user's local time.</summary>
        /// <param name="eveTime">The Eve date/time.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime ToLocalTime(this DateTime eveTime)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(eveTime);
        }

        /// <summary>Converts a date that is in the user's local time to Eve Time.</summary>
        /// <param name="localTime">The local date/time.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime ToEveTime(this DateTime localTime)
        {
            return TimeZone.CurrentTimeZone.ToUniversalTime(localTime);
        }

        #endregion
    }
}