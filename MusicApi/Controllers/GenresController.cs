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
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenresController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> Get()
        {
            var GenreList = await _unitOfWork.Genres.FindAll();
            if (!GenreList.Any())
                return await Task.Run(NotFound);
            var genreReadDtoList = _mapper.Map<IEnumerable<GenreReadDto>>(GenreList);
            return Ok(genreReadDtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreReadDto>> Get(int id)
        {
            var genre = await _unitOfWork.Genres.Find(id);
            if (genre == null)
                return await Task.Run(NotFound);
            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return await Task.Run(() => Ok(genreReadDto));
        }

        [HttpPost]
        public async Task<ActionResult<GenreReadDto>> Post(GenreCreateDto genreCreateDto)
        {
            var genre = _mapper.Map<Genre>(genreCreateDto);
            await _unitOfWork.Genres.Create(genre);
            await _unitOfWork.Save();

            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return await Task.Run(() => CreatedAtAction(nameof(Get), new {Id = genreReadDto.Id}, genreReadDto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenreReadDto>> Put(int id, GenreUpdateDto genreUpdateDto)
        {
            var genre = await _unitOfWork.Genres.Find(id);
            if (genre == null)
                return await Task.Run(NotFound);
            _mapper.Map(genreUpdateDto, genre);

            await _unitOfWork.Genres.Update(genre);
            await _unitOfWork.Save();

            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return await Task.Run(() => Ok(genreReadDto));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GenreReadDto>> Patch(int id, JsonPatchDocument<GenreUpdateDto> patchDocument)
        {
            var genre = await _unitOfWork.Genres.Find(id);
            if (genre == null)
                return await Task.Run(NotFound);
            var genreToPatch = _mapper.Map<GenreUpdateDto>(genre);
            patchDocument.ApplyTo(genreToPatch, ModelState);
            if (!TryValidateModel(genreToPatch))
                return ValidationProblem(ModelState);
            _mapper.Map(genreToPatch, genre);
            
            var genreReadDto = _mapper.Map<GenreReadDto>(genre);
            return await Task.Run(() => Ok(genreReadDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await _unitOfWork.Genres.Find(id);
            if (genre == null)
                return await Task.Run(NotFound);
            await _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.Save();
            return await Task.Run(NoContent);
        }
        
        [HttpGet("{id}/Songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs(int id)
        {
            var songsByGenreList = await _unitOfWork.Songs.FindByCondition(x => x.GenreId == id);
            if (!songsByGenreList.Any())
                return await Task.Run(NotFound);
            var songsByGenreReadDto = _mapper.Map<IEnumerable<SongReadDto>>(songsByGenreList);
            return Ok(songsByGenreReadDto);
        }
        
        [HttpGet("{id}/Songs/{songId}")]
        public async Task<ActionResult<GenreReadDto>> Get(int id, int songId)
        {
            var song = await _unitOfWork.Songs.Find(songId);
            if (song == null) 
                return await Task.Run(NotFound);
            var songReadDto = _mapper.Map<SongReadDto>(song);
            return await Task.Run(() => Ok(songReadDto));

        }
        
    }
}