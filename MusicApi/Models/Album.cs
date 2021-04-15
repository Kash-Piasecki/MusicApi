using System.Collections.Generic;

namespace MusicApi.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public IEnumerable<Song> Songs { get; set; }
    }
}