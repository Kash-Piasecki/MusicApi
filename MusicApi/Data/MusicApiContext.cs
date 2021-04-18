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
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<SongPlaylist> SongPlaylist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Song>().HasOne(x => x.Album).WithMany(x => x.Songs).HasForeignKey(x => x.AlbumId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Song>().HasOne(x => x.Artist).WithMany(x => x.Songs).HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Song>().HasOne(x => x.Genre).WithMany(x => x.Songs).HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Album>().HasOne(x => x.Artist).WithMany(x => x.Albums).HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SongPlaylist>().HasOne(x => x.Song).WithMany(x => x.SongPlaylist)
                .HasForeignKey(x => x.SongId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SongPlaylist>().HasOne(x => x.PlayList).WithMany(x => x.SongPlaylist)
                .HasForeignKey(x => x.PlaylistId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}