using System;

namespace MagicMirror.Business.Models
{
    public class DateModel
    {
        public string DayShort => DateTime.Now.DayOfWeek.ToShortDayNotation();

        public string DateFull => DateTime.Now.ToString("ddddd, MMMM d");

        public string Time => DateTime.Now.ToString("HH:mm");
    }
}