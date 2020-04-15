using System;
using Liki24.Contracts.Interfaces;

namespace Liki24.Contracts.Models
{
    public class GetDeliveryIntervalsForHorizonRequest : ICacheKey<GetDeliveryIntervalsForHorizonRequest>
    {
        public DateTime CurrentDate { get; set; }

        public int Horizon { get; set; }

        public bool Equals(GetDeliveryIntervalsForHorizonRequest other)
        {
            return CurrentDate.Equals(other.CurrentDate) && Horizon == other.Horizon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GetDeliveryIntervalsForHorizonRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CurrentDate.GetHashCode() * 397) ^ Horizon;
            }
        }
    }
}