using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Liki24.Contracts.Interfaces;
using Liki24.DAL;

namespace Liki24.BL.Base
{
    public class BaseManager<TDto, TDb> : IManager<TDto>
    {
        private readonly IRepository<TDb> _repository;
        private readonly IMapper _mapper;

        protected BaseManager(IRepository<TDb> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<TDto>> GetAllAsync()
        {
            return _mapper.Map<ICollection<TDto>>((await _repository.GetAllAsync()).ToList());
        }

        public async Task<TDto> GetAsync(int id)
        {
            return _mapper.Map<TDto>(await _repository.GetAsync(id));
        }

        public async Task<TDto> AddAsync(TDto entity)
        {
            var dbEntity = _mapper.Map<TDb>(entity);
            var result = await _repository.AddAsync(dbEntity);
            return _mapper.Map<TDto>(result);
        }

        public async Task<bool> UpdateAsync(TDto entity)
        {
            var dbEntity = _mapper.Map<TDb>(entity);
            return await _repository.UpdateAsync(dbEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}