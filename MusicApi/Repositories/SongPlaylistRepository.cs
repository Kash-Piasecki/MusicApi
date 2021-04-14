using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    class SongPlaylistRepository : Repository<SongPlaylist>, ISongPlaylistRepository
    {
        public SongPlaylistRepository(MusicApiContext db) : base(db)
        {
        }
    }
}