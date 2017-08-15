using Acme.Generic;
using System;

namespace MagicMirror.Business.Models
{
    public class DateModel
    {
        public string DayShort => DateTime.Now.DayOfWeek.ToShortDayNotation();

        public string DateFull => DateTime.Now.ToString("dd MMMM yyyy");
    }
}