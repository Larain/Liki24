using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liki24.Contracts.Interfaces
{
    public interface IManager<T>
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}