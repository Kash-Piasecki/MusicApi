using System.Collections.Generic;

namespace MusicApi.DTOs
{
    public class PlaylistReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SongReadDto> Songs { get; set; }
    }
}