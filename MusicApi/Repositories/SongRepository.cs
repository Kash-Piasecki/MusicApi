using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    public class SongRepository : Repository<Song>, ISongRepository
    {
        public SongRepository(MusicApiContext db) : base(db)
        {
        }
    }
}