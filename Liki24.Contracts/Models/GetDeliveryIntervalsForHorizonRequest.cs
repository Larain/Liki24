using System;

namespace Liki24.Contracts.Models
{
    public class GetDeliveryIntervalsForHorizonRequest
    {
        public DateTime CurrentDate { get; set; }
        public uint Horizon { get; set; }
    }
}