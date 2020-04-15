using System;
using System.Collections.Generic;

namespace Liki24.DAL.Models
{
    public class DeliveryInterval
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public uint? AvailabilityDeltaHours { get; set; }
        public TimeSpan AvailableFrom { get; set; }
        public TimeSpan AvailableTo { get; set; }
        public DeliveryIntervalType Type { get; set; }
        public ICollection<DayOfWeek> AvailableDaysOfWeek { get; set; }
        public TimeSpan ExpectedFrom { get; set; }
        public TimeSpan ExpectedTo { get; set; }
    }
}
