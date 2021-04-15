using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicApi.Data;
using MusicApi.DTOs;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public SongsController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<SongsController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongReadDto>>> Get()
        {
            var songs = await _unitOfWork.Songs.FindAll();
            if (songs.Any())
            {
                var songReadDto = _mapper.Map<IEnumerable<SongReadDto>>(songs);
                _logger.LogInformation("Entities Found");
                return await Task.Run(() => Ok(songReadDto));
            }

            _logger.LogWarning("No Entities Found");
            return await Task.Run(NotFound);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongReadDto>> Get(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song != null)
            {
                var songReadDto = _mapper.Map<SongReadDto>(song);
                _logger.LogInformation("Entity found");
                return await Task.Run(() => Ok(songReadDto));
            }

            _logger.LogWarning("Wrong entity Id.");
            return await Task.Run(NotFound);
        }

        [HttpPost]
        public async Task<ActionResult<SongReadDto>> Post(SongCreateDto songCreateDto)
        {
            var song = _mapper.Map<Song>(songCreateDto);
            await _unitOfWork.Songs.Create(song);
            await _unitOfWork.Save();
            var songReadDto = _mapper.Map<SongReadDto>(song);
            return await Task.Run(() => CreatedAtAction(nameof(Get), new {Id = songReadDto.Id}, songReadDto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SongReadDto>> Update(int id, SongUpdateDto songUpdateDto)
        {
            var song = await _unitOfWork.Songs.Find(id);

            if (song == null)
            {
                return await Task.Run(NotFound);
            }

            _mapper.Map(songUpdateDto, song);

            await _unitOfWork.Songs.Update(song);
            await _unitOfWork.Save();

            var songReadDto = _mapper.Map<SongReadDto>(song);
            return await Task.Run(() => Ok(songReadDto));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<SongReadDto>> Patch(int id, JsonPatchDocument<SongUpdateDto> patchDocument)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song == null)
            {
                return await Task.Run(NotFound);
            }

            var songToPatch = _mapper.Map<SongUpdateDto>(song);
            patchDocument.ApplyTo(songToPatch, ModelState);
            if (!TryValidateModel(songToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(songToPatch, song);

            await _unitOfWork.Songs.Update(song);
            await _unitOfWork.Save();

            var songReadDto = _mapper.Map<SongReadDto>(song);
            return await Task.Run((() => Ok(songReadDto)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song == null)
            {
                return await Task.Run(NotFound);
            }
            await _unitOfWork.Songs.Delete(song);
            await _unitOfWork.Save();
            return await Task.Run(NoContent);
        }
    }
}