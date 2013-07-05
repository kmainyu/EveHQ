// -----------------------------------------------------------------------
// <copyright file="EveHQTraceLogger.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using EveHQ.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EveHqTraceLogger:TraceListener
    {
        private Stream _outputStream;

        private const string OutputLineFormat = "{0}:{1}\r\n";

        private const string MessageCategoryFormat = "{0}:{1}";

        public EveHqTraceLogger(Stream loggingStream)
        {
            _outputStream = loggingStream;
        }

        public override void WriteLine(string message, string category)
        {
            this.WriteLine(MessageCategoryFormat.FormatInvariant(category, message));
        }


        public override void Write(string message)
        {
            if (_outputStream.CanWrite)
            {
                var bytes = GetMessageBytes(message);
                _outputStream.Write(bytes, 0, bytes.Length);
            }
        }

        public override void WriteLine(string message)
        {
            if (_outputStream.CanWrite)
            {
                var bytes = GetMessageBytes(OutputLineFormat.FormatInvariant(DateTimeOffset.Now, message));
                _outputStream.Write(bytes, 0, bytes.Length);
            }
        }

        public override void Flush()
        {
            _outputStream.Flush();
            base.Flush();
        }
        private static byte[] GetMessageBytes(string message)
        {

            return UTF8Encoding.UTF8.GetBytes(message);
        }

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
    }
}
