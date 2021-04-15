namespace MusicApi.DTOs
{
    public class SongCreateDto
    {
        public string Name { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
    }
}