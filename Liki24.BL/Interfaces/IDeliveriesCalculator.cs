using System.Collections.Generic;
using Liki24.Contracts.Models;

namespace Liki24.BL.Interfaces
{
    public interface IDeliveriesCalculator
    {
        ICollection<ClientDeliveryInterval> GetDeliveriesForHorizon(GetDeliveryIntervalsForHorizonRequest request);
    }
}