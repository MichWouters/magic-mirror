using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.Generic.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Converts an amount of minutes into time as X hours and Y minutes
        /// </summary>
        public static string ConvertMinutesToHoursAndMinutes(this int minutes)
        {
            double hours = minutes / 60;
            double restMinutes = minutes % 60;

            string result = (restMinutes != 0) ? $"{hours} hours and {restMinutes} minutes" : $"{hours} hours";
            return result;
        }
    }
}
