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
using System.Text;

namespace EveHQ.Common.Extensions
{
    /// <summary>
    ///     extension methods for processing exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Public Methods and Operators

        /// <summary>Formats an exception into a string containing all of the inner and aggregate exception details.</summary>
        /// <param name="exception">the exception to format.</param>
        /// <returns>The <see cref="string" />.</returns>
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