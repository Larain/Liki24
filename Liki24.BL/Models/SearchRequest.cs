using System;

namespace Liki24.BL.Models
{
    public class SearchRequest
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? LookFrom { get; set; }
        public TimeSpan? LookTo { get; set; }
    }
}