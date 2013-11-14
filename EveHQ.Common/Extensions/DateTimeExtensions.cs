
namespace EveHQ.Common.Extensions
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static string ToInvariantString(this DateTime time)
        {
            return time.ToString(CultureInfo.InvariantCulture);
        }
    }
}