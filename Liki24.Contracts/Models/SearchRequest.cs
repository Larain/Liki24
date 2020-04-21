using System;
using Liki24.Contracts.Interfaces;

namespace Liki24.Contracts.Models
{
    public class SearchRequest : ICacheKey, IEquatable<SearchRequest>
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? LookFrom { get; set; }
        public TimeSpan? LookTo { get; set; }
        public bool? HasDelta { get; set; }
        public string Key => $"{DayOfWeek};LookTo={LookTo};LookFrom={LookFrom};HasDelta={HasDelta};";

        public bool Equals(SearchRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DayOfWeek == other.DayOfWeek && Nullable.Equals(LookFrom, other.LookFrom) && Nullable.Equals(LookTo, other.LookTo) && HasDelta == other.HasDelta;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) DayOfWeek;
                hashCode = (hashCode * 397) ^ LookFrom.GetHashCode();
                hashCode = (hashCode * 397) ^ LookTo.GetHashCode();
                hashCode = (hashCode * 397) ^ HasDelta.GetHashCode();
                return hashCode;
            }
        }
    }
}