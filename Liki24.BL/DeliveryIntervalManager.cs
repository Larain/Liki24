using AutoMapper;
using Liki24.BL.Base;
using Liki24.Contracts.Models;
using Liki24.DAL;
using Liki24.DAL.Models;

namespace Liki24.BL
{
    public class DeliveryIntervalManager : BaseManager<DeliveryIntervalDto, DeliveryInterval>
    {
        public DeliveryIntervalManager(IRepository<DeliveryInterval> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}