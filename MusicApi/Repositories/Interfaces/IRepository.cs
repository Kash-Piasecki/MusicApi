using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> FindAll();
        public Task<T> Find(int id);
        public Task Create(T entity);
        public Task Update(T entity);
        public Task Delete(T entity);
    }
}