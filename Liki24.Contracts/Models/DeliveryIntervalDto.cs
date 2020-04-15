using System.Collections.Generic;

namespace Liki24.Contracts.Models
{
    public class DeliveryIntervalDto : DeliveryIntervalBase
    {
        public int Id { get; set; }
        public List<Value> AvailableDaysOfWeek { get; set; }
    }
}