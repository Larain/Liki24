using System;
using Liki24.Contracts.Interfaces;

namespace Liki24.Contracts.Models
{
    public class SearchRequest : ICacheKey
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? LookFrom { get; set; }
        public TimeSpan? LookTo { get; set; }
        public string Key => $"{DayOfWeek} + LookTo = {LookTo} + LookFrom = {LookFrom}";
    }
}