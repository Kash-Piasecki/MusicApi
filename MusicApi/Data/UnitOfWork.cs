using System.Threading.Tasks;
using MusicApi.Repositories;

namespace MusicApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicApiContext _db;
        public ISongRepository Songs { get; set; }
        public IAlbumRepository Albums { get; set; }
        public IGenreRepository Genres { get; set; }
        public IArtistRepository Artists { get; set; }
        public IPlaylistRepository Playlists { get; set; }
        public ISongPlaylistRepository SongPlaylist { get; set; }

        public UnitOfWork(MusicApiContext db)
        {
            _db = db;
            Songs = new SongRepository(_db);
            Albums = new AlbumRepository(_db);
            Genres = new GenreRepository(_db);
            Artists = new ArtistRepository(_db);
            Playlists = new PlaylistRepository(_db);
            SongPlaylist = new SongPlaylistRepository(_db);
        }


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}