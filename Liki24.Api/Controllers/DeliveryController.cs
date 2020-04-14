using System.Collections.Generic;
using AutoMapper;
using Liki24.Api.Controllers.Models;
using Liki24.BL;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Liki24.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveriesCalculator _iDeliveriesCalculator;
        private static readonly IMapper Mapper;

        static DeliveryController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GetDeliveryIntervalsForHorizonModel, GetDeliveryIntervalsForHorizonRequest>();
            });
            Mapper = config.CreateMapper();
        }

        public DeliveryController(IDeliveriesCalculator iDeliveriesCalculator)
        {
            _iDeliveriesCalculator = iDeliveriesCalculator;
        }

        // GET: api/Delivery
        [HttpPost]
        [Route("horizon")]
        public IEnumerable<ClientDeliveryInterval> Calculate(GetDeliveryIntervalsForHorizonModel model)
        {
            var request = Mapper.Map<GetDeliveryIntervalsForHorizonRequest>(model);
            return _iDeliveriesCalculator.GetDeliveriesForHorizon(request);
        }
    }
}
