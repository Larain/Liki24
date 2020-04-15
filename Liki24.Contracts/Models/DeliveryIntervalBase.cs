namespace Liki24.Contracts.Models
{
    public class DeliveryIntervalBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public uint? AvailabilityDeltaHours { get; set; }
        public string AvailableFrom { get; set; }
        public string AvailableTo { get; set; }
        public Value Type { get; set; }
        public string ExpectedFrom { get; set; }
        public string ExpectedTo { get; set; }
    }
}