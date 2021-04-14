using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository
    {
        public AlbumRepository(MusicApiContext db) : base(db)
        {
        }
    }
}