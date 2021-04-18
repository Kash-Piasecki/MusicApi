using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.DTOs;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaylistsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistReadDto>>> Get()
        {
            var playlistsList = await _unitOfWork.Playlists.FindAll();
            if (!playlistsList.Any())
                return await Task.Run(NotFound);
            var playlistReadDtoList = _mapper.Map<IEnumerable<PlaylistReadDto>>(playlistsList);
            return Ok(playlistReadDtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistReadDto>> Get(int id)
        {
            var playlist = await _unitOfWork.Playlists.Find(id);
            if (playlist == null)
                return await Task.Run(NotFound);
            var playlistReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return await Task.Run(() => Ok(playlistReadDto));
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistReadDto>> Post(PlaylistCreateDto playlistCreateDto)
        {
            var playlist = _mapper.Map<PlayList>(playlistCreateDto);
            await _unitOfWork.Playlists.Create(playlist);
            await _unitOfWork.Save();

            var playlistReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return await Task.Run(() => CreatedAtAction(nameof(Get), new {Id = playlistReadDto.Id}, playlistReadDto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlaylistReadDto>> Put(int id, PlaylistUpdateDto playlistUpdateDto)
        {
            var playlist = await _unitOfWork.Playlists.Find(id);
            if (playlist == null)
                return await Task.Run(NotFound);
            _mapper.Map(playlistUpdateDto, playlist);

            await _unitOfWork.Playlists.Update(playlist);
            await _unitOfWork.Save();

            var genreReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return await Task.Run(() => Ok(genreReadDto));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PlaylistReadDto>> Patch(int id,
            JsonPatchDocument<PlaylistUpdateDto> patchDocument)
        {
            var playlist = await _unitOfWork.Playlists.Find(id);
            if (playlist == null)
                return await Task.Run(NotFound);
            var playlistToPatch = _mapper.Map<PlaylistUpdateDto>(playlist);
            patchDocument.ApplyTo(playlistToPatch, ModelState);
            if (!TryValidateModel(playlistToPatch))
                return ValidationProblem(ModelState);
            _mapper.Map(playlistToPatch, playlist);

            var playlistReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return await Task.Run(() => Ok(playlistReadDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var playlist = await _unitOfWork.Playlists.Find(id);
            if (playlist == null)
                return await Task.Run(NotFound);
            await _unitOfWork.Playlists.Delete(playlist);
            await _unitOfWork.Save();
            return await Task.Run(NoContent);
        }

        [HttpGet("{id}/Songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs(int id)
        {
            var songPlaylistList = await _unitOfWork.SongPlaylist.FindByCondition(x => x.PlaylistId == id);
            if (!songPlaylistList.Any())
                return await Task.Run(NotFound);
            var songsByPlaylistList = await GetSongsByPlaylist(songPlaylistList, id);

            var songsByPlaylistDto = _mapper.Map<IEnumerable<SongReadDto>>(songsByPlaylistList);
            return Ok(songsByPlaylistDto);
        }

        [HttpGet("{id}/Songs/{songId}")]
        public async Task<ActionResult<GenreReadDto>> Get(int id, int songId)
        {
            var songPlaylist =
                await _unitOfWork.SongPlaylist.FindByCondition(x => x.PlaylistId == id && x.SongId == songId);
            if (songPlaylist.Any())
            {
                var first = songPlaylist.FirstOrDefault();
                var song = await _unitOfWork.Songs.Find(first.SongId);
                var songReadDto = _mapper.Map<SongReadDto>(song);
                return await Task.Run(() => Ok(songReadDto));
            }

            return await Task.Run(NotFound);
        }
        
        [HttpPost("{id}/Songs/{songId}")]
        public async Task<ActionResult> PostSongInPlaylist(int id, int songId)
        {
            var song = await _unitOfWork.Songs.Find(songId);
            var playlist = await _unitOfWork.Playlists.Find(id);
            if (song == null || playlist == null)
            {
                return await Task.Run(NotFound);
            }

            var newSongPlaylist = new SongPlaylist()
            {
                PlaylistId = id,
                SongId = songId
            };
            await _unitOfWork.SongPlaylist.Create(newSongPlaylist);
            await _unitOfWork.Save();
            return await Task.Run(() => Ok());
        }

        [HttpDelete("{id}/Songs/{songId}")]
        public async Task<ActionResult> DeleteSongFromPlaylist(int id, int songId)
        {
            var songPlaylistList = await _unitOfWork.SongPlaylist
                .FindByCondition(x => x.PlaylistId == id && x.SongId == songId);
            var songPlaylit = songPlaylistList.FirstOrDefault();
            if (songPlaylit == null)
            {
                return await Task.Run(NotFound);
            }

            await _unitOfWork.SongPlaylist.Delete(songPlaylit);
            await _unitOfWork.Save();
            return await Task.Run(Ok);
        }
        
        
        
        private async Task<IEnumerable<Song>> GetSongsByPlaylist(IEnumerable<SongPlaylist> list, int id)
        {
            var songs = new List<Song>();
            foreach (var songPlaylist in list)
                songs.Add(await _unitOfWork.Songs.Find(songPlaylist.SongId));

            return songs;
        }
        
    }
}