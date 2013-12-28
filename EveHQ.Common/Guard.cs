
namespace EveHQ.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
 
    public static class Guard
    {
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "Only used in .net4 and higher.")]
        public static void Against(bool test, string message = null)
        {
            if (test)
            {
                throw new ArgumentException(string.IsNullOrWhiteSpace(message) ? "Guard check failed" : message);
            }
        }

          [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "Only used in .net4 and higher.")]
        public static void Ensure(bool test, string message = null)
        {
            Against(!test, message);
        }
    }
}