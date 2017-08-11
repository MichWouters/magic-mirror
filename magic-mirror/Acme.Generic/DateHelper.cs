using System;

namespace Acme.Generic
{
    public static class DateHelper
    {
        /// <summary>
        /// Calculates a DateTime object by adding a number of seconds to 1/1/1970
        /// </summary>
        /// <param name="timestamp">The amount of seconds passed since the first of January 1970</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// Calculates the time in seconds between a given date and the first of January 1970
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Seconds since 1/1/1970</returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}