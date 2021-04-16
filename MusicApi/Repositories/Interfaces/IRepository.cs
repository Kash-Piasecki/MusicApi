using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MusicApi.Pagination;

namespace MusicApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> FindAll();
        public Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> FindWithPagingFilter(PaginationFilter filter);
        public Task<int> Count();
        public Task<T> Find(int id);
        public Task Create(T entity);
        public Task Update(T entity);
        public Task Delete(T entity);
    }
}