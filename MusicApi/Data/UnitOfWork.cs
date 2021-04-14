using System.Threading.Tasks;
using MusicApi.Repositories;

namespace MusicApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicApiContext _db;
        public ISongRepository Songs { get; set; }

        public UnitOfWork(MusicApiContext db)
        {
            _db = db;
            Songs = new SongRepository(_db);
        }
        
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}