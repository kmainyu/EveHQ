// ===========================================================================
// <copyright file="ExceptionExtensions.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ExceptionExtensions.cs), is part of EveHQ.
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
    using System.Text;

    /// <summary>
    ///     extension methods for processing exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Public Methods and Operators

        /// <summary>Formats an exception into a string containing all of the inner and aggregate exception details.</summary>
        /// <param name="exception">the exception to format.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string FormatException(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var output = new StringBuilder();

            var aggException = exception as AggregateException;

            if (aggException != null)
            {
                output.Append("*****START Aggregate Exception Details*****\r\n");
                output.AppendFormat("Message: {0}\r\n", aggException.Message);
                output.AppendFormat("Source: {0}\r\n", aggException.Source);
                output.AppendFormat("StackTrace: {0}\r\n", aggException.StackTrace);

                output.Append("*****Inner Exceptions*****\r\n");
                foreach (Exception innerException in aggException.InnerExceptions)
                {
                    output.Append(innerException.FormatException());
                }

                output.Append("*****END Aggregate Exception Details*****\r\n");
            }
            else
            {
                output.Append("*****START Exception Details*****\r\n");
                output.AppendFormat("Message: {0}\r\n", exception.Message);
                output.AppendFormat("Source: {0}\r\n", exception.Source);
                output.AppendFormat("StackTrace: {0}\r\n", exception.StackTrace);
                if (exception.InnerException != null)
                {
                    output.Append("*****Inner Exception*****\r\n");
                    output.Append(exception.InnerException.FormatException());
                }

                output.Append("*****END Exception Details*****\r\n");
            }

            return output.ToString();
        }

        #endregion
    }
}