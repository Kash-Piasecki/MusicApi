using AutoMapper;
using MusicApi.Models;

namespace MusicApi.DTOs
{
    public class MusicApiProfiles : Profile
    {
        public MusicApiProfiles()
        {
            CreateMap<Models.Song, SongReadDto>();
            CreateMap<SongCreateDto, Models.Song>();
            CreateMap<SongUpdateDto, Models.Song>();
            CreateMap<Models.Song, SongUpdateDto>();
            
            CreateMap<Artist, ArtistReadDto>();
            CreateMap<ArtistCreateDto, Artist>();
            CreateMap<ArtistUpdateDto, Artist>();
            CreateMap<Artist, ArtistUpdateDto>();

            CreateMap<Genre, GenreReadDto>();
            CreateMap<GenreCreateDto, Genre>();
            CreateMap<GenreUpdateDto, Genre>();
            CreateMap<Genre, GenreUpdateDto>();
            
            CreateMap<PlayList, PlaylistReadDto>();
            CreateMap<PlaylistCreateDto, PlayList>();
            CreateMap<PlaylistUpdateDto, PlayList>();
            CreateMap<PlayList, PlaylistUpdateDto>();
        }
        
    }
}