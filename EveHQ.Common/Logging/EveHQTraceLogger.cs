// ===========================================================================
// <copyright file="EveHQTraceLogger.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (EveHQTraceLogger.cs), is part of EveHQ.
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
namespace EveHQ.Common.Logging
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using EveHQ.Common.Extensions;

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

        /// <summary>Initializes a new instance of the <see cref="EveHQTraceLogger"/> class.</summary>
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
            this.WriteLine(MessageCategoryFormat.FormatInvariant(category, message));
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
                _outputStream.Flush();
                _outputStream.Close();
                _outputStream.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>The get message bytes.</summary>
        /// <param name="message">The message.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        private static byte[] GetMessageBytes(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        #endregion
    }
}