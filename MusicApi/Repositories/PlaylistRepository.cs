using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    internal class PlaylistRepository : Repository<PlayList>, IPlaylistRepository
    {
        public PlaylistRepository(MusicApiContext db) : base(db)
        {
        }
    }
}