using System.Threading.Tasks;
using MusicApi.Repositories;

namespace MusicApi.Data
{
    public interface IUnitOfWork
    {
        public ISongRepository Songs { get; set; }
        public IAlbumRepository Albums { get; set; }
        public IGenreRepository Genres { get; set; }
        public IArtistRepository Artists { get; set; }
        public IPlaylistRepository Playlists { get; set; }
        Task Save();
    }
}