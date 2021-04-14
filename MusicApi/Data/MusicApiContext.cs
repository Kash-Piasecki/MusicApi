using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data
{
    public class MusicApiContext : DbContext
    {
        public MusicApiContext(DbContextOptions<MusicApiContext> options) : base(options)
        {
            
        }

        public DbSet<Song> Songs { get; set; }
    }
}