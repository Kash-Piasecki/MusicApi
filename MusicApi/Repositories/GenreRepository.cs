using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MusicApiContext db) : base(db)
        {
        }
    }
}