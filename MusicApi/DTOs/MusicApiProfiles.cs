using AutoMapper;
using MusicApi.Models;

namespace MusicApi.DTOs
{
    public class MusicApiProfiles : Profile
    {
        public MusicApiProfiles()
        {
            CreateMap<Song, SongReadDto>();
            CreateMap<SongCreateDto, Song>();
            CreateMap<SongUpdateDto, Song>();
            CreateMap<Song, SongUpdateDto>();
            
            CreateMap<Artist, ArtistReadDto>();
            CreateMap<ArtistCreateDto, Artist>();
            CreateMap<ArtistUpdateDto, Artist>();
            CreateMap<Artist, ArtistUpdateDto>();
        }
        
    }
}