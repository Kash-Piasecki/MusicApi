using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;

namespace MusicApi.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public MusicApiContext _db { get; set; }

        protected Repository(MusicApiContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _db.Set<T>().ToListAsync();
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