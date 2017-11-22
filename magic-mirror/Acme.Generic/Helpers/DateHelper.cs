using System;

namespace Acme.Generic
{
    public class DateHelper
    {
        public static string CurrentDayShort => DateTime.Now.DayOfWeek.ToShortDayNotation();

        public static string CurrentTimeFull => DateTime.Now.ToString("ddddd, MMMM d");

        public static string CurrentTime => DateTime.Now.ToString("HH:mm");
    }
}