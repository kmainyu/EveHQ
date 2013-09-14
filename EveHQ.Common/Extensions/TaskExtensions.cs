// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (TaskExtensions.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================
namespace EveHQ.Common.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>Attempts to run the task, and captures any failures to the trace log</summary>
        /// <param name="factory">The factory.</param>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task TryRun(this TaskFactory factory, Action action)
        {
            if (factory != null)
            {
                return factory.StartNew(action).ContinueWith((t) => VerifyTaskCompletedSuccessFully(t));
            }

            return null;
        }

        /// <summary>Attempts to run the task, and captures any failures to the trace log</summary>
        /// <param name="factory">The factory.</param>
        /// <param name="action">The action.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task<T> TryRun<T>(this TaskFactory<T> factory, Func<T> action)
        {
            if (factory != null)
            {
                return factory.StartNew(action).ContinueWith((t) => VerifyTaskCompletedSuccessFully(t));
            }

            return null;
        }

        /// <summary>The verify task completed success fully.</summary>
        /// <param name="task">The task.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static Task VerifyTaskCompletedSuccessFully(Task task)
        {
            if (task.IsFaulted && task.Exception != null)
            {
                Trace.TraceError(task.Exception.FormatException());
                Trace.Flush();
            }

            return task;
        }

        /// <summary>The verify task completed success fully.</summary>
        /// <param name="task">The task.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="T"/>.</returns>
        private static T VerifyTaskCompletedSuccessFully<T>(Task<T> task)
        {
            if (task.IsFaulted && task.Exception != null)
            {
                Trace.TraceError(task.Exception.FormatException());
                Trace.Flush();
                return default(T);
            }

            return task.Result;
        }
    }
}