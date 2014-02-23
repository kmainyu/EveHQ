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
using System.Diagnostics;
using System.IO;
using System.Text;
using EveHQ.Common.Extensions;

namespace EveHQ.Common.Logging
{
    /// <summary>
    ///     ETW listener for EveHQ to log events to file.
    /// </summary>
    public class EveHQTraceLogger : TraceListener
    {
        #region Constants

        /// <summary>The message category format.</summary>
        private const string MessageCategoryFormat = "{0}:{1}";

        /// <summary>The output line format.</summary>
        private const string OutputLineFormat = "{0}:{1}\r\n";

        #endregion

        #region Fields

        /// <summary>The _output stream.</summary>
        private readonly Stream _outputStream;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="EveHQTraceLogger" /> class.</summary>
        /// <param name="loggingStream">The logging stream.</param>
        public EveHQTraceLogger(Stream loggingStream)
        {
            _outputStream = loggingStream;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>The flush.</summary>
        public override void Flush()
        {
            _outputStream.Flush();
            base.Flush();
        }

        /// <summary>The write.</summary>
        /// <param name="message">The message.</param>
        public override void Write(string message)
        {
            if (_outputStream.CanWrite)
            {
                byte[] bytes = GetMessageBytes(message);
                _outputStream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>The write line.</summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        public override void WriteLine(string message, string category)
        {
            WriteLine(MessageCategoryFormat.FormatInvariant(category, message));
        }

        /// <summary>The write line.</summary>
        /// <param name="message">The message.</param>
        public override void WriteLine(string message)
        {
            if (_outputStream.CanWrite)
            {
                byte[] bytes = GetMessageBytes(OutputLineFormat.FormatInvariant(DateTimeOffset.Now, message));
                _outputStream.Write(bytes, 0, bytes.Length);
            }
        }

        #endregion

        #region Methods

        /// <summary>The dispose.</summary>
        /// <param name="disposing">The disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_outputStream != null && _outputStream.CanWrite)
                {
                    _outputStream.Flush();
                    _outputStream.Close();
                    _outputStream.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>The get message bytes.</summary>
        /// <param name="message">The message.</param>
        /// <returns>The <see cref="byte" />.</returns>
        private static byte[] GetMessageBytes(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        #endregion
    }
}