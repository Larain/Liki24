namespace Liki24.Contracts.Models
{
    public class ClientDeliveryInterval : DeliveryIntervalBase
    {
        public Value DayOfWeek { get; set; }
        public bool Available { get; set; }
    }
}