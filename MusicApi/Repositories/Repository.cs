using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Pagination;

namespace MusicApi.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected Repository(MusicApiContext db)
        {
            _db = db;
        }

        public MusicApiContext _db { get; set; }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindWithPagingFilter(PaginationFilter filter)
        {
            return await _db.Set<T>().Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _db.Set<T>().CountAsync();
        }

        public async Task<T> Find(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task Create(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            await Task.Run(() => Console.WriteLine("Update"));
        }

        public async Task Delete(T entity)
        {
            await Task.Run(() => _db.Remove(entity));
        }
    }
}