using System;
using System.Collections.Generic;
using Liki24.Contracts.Interfaces;

namespace Liki24.Contracts.Models
{
    public class GetDeliveryIntervalsForHorizonRequest : ICacheKey
    {
        public DateTime CurrentDate { get; set; }
        public uint Horizon { get; set; }
        public string Key => Horizon.ToString() + CurrentDate;
    }
}