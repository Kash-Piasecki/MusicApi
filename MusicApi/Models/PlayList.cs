using System.Collections.Generic;

namespace MusicApi.Models
{
    public class PlayList
    {
        public int Id { get; set; }
        public IEnumerable<SongPlaylist> SongPlaylist { get; set; }
    }
}