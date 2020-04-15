using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Liki24.Api.Controllers.Models;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Liki24.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminDeliveryController : ControllerBase
    {
        private readonly IManager<DeliveryIntervalDto> _repository;
        private static readonly IMapper Mapper;

        static AdminDeliveryController()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DeliveryIntervalForCreateModel, DeliveryIntervalDto>();
                    cfg.CreateMap<DeliveryIntervalForEditModel, DeliveryIntervalDto>();
                });
            Mapper = config.CreateMapper();
        }

        public AdminDeliveryController(IManager<DeliveryIntervalDto> repository)
        {
            _repository = repository;
        }

        // GET: api/AdminDelivery
        [HttpGet("")]
        public async Task<IEnumerable<DeliveryIntervalDto>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/AdminDelivery/5
        [HttpGet("{id}")]
        public async Task<DeliveryIntervalDto> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        // POST: api/AdminDelivery
        [HttpPost]
        public async Task<DeliveryIntervalDto> PostAsync(DeliveryIntervalForCreateModel value)
        {
            return await _repository.AddAsync(Mapper.Map<DeliveryIntervalDto>(value));
        }

        // PUT: api/AdminDelivery/5
        [HttpPut]
        public async Task<bool> PutAsync(DeliveryIntervalForEditModel value)
        {
            return await _repository.UpdateAsync(Mapper.Map<DeliveryIntervalDto>(value));
        }

        // DELETE: api/AdminDelivery/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
