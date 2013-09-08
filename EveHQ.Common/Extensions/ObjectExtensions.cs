// -----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ObjectExtensions
    {
        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj, CultureInfo.InvariantCulture);
        }

        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj, CultureInfo.InvariantCulture);
        }

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj, CultureInfo.InvariantCulture);
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
        }
    }
}
