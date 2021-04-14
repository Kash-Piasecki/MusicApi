
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Album
    {
        public int Id { get; set; }
        
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        
        public IEnumerable<Song> Songs { get; set; }
    }
}