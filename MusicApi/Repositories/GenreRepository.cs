using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Repositories
{
    internal class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MusicApiContext db) : base(db)
        {
        }
    }
}