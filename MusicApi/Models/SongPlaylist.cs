using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class SongPlaylist
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int PlaylistId { get; set; }
        public PlayList PlayList { get; set; }
    }
}