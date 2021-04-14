using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(MusicApiContext db) : base(db)
        {
        }
    }
}