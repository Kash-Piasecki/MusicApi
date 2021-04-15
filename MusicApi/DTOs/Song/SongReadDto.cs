namespace MusicApi.DTOs
{
    public class SongReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GenreId { get; set; }
        // public Genre Genre { get; set; }
        public int ArtistId { get; set; }
        // public Artist Artist { get; set; }
        public int AlbumId { get; set; }
        // public Album Album { get; set; }
        // public IEnumerable<SongPlaylist> SongPlaylist { get; set; }
    }
}