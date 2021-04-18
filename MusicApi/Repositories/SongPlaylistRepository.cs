using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    internal class SongPlaylistRepository : Repository<SongPlaylist>, ISongPlaylistRepository
    {
        public SongPlaylistRepository(MusicApiContext db) : base(db)
        {
        }
    }
}