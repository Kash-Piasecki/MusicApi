using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    internal class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(MusicApiContext db) : base(db)
        {
        }
    }
}