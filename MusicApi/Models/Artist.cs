using System.Collections.Generic;

namespace MusicApi.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }
}