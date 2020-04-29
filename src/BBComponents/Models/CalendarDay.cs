using System;
using System.Text;

namespace BBComponents.Models
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public bool IsAnotherMonth { get; set; }

        public int Day => Date.Day;
        public bool IsWeekEnd => Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday;

        public string ModificatorClass
        {

            get
            {
                var sb = new StringBuilder();

                if (IsActive)
                {
                    sb.Append("bb-calendar__day_active");
                }

                if (IsAnotherMonth)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append("bb-calendar__day_another");
                }

                if (IsWeekEnd)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append("bb-calendar__day_weekend");
                }

                return sb.ToString();
            }

        }
    }
}
