using System.Linq;
using System.Threading.Tasks;

namespace Liki24.DAL
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}