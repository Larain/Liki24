using System;

namespace Liki24.BL.Helpers
{
    public static class Helpers
    {
        public static bool IsValidDateTime(this string value)
        {
            if (!TimeSpan.TryParse(value, out var timeSpan)) return false;
            return timeSpan.TotalMinutes <= TimeSpan.FromHours(24).TotalMinutes && timeSpan.TotalMinutes >= 0;
        }

        public static DayOfWeek GetNextDay(this DayOfWeek currentDay)
        {
            return (DayOfWeek) (((int) currentDay + 1) % 7); // 7 day per week
        }
    }
}