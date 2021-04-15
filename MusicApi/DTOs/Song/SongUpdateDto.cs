namespace MusicApi.DTOs
{
    public class SongUpdateDto
    {
        public string Name { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
    }
}