using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            return await Task.Run(() => NotFound());
        }

        [HttpGet("{id}", Name = "Get")]
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
            return await Task.Run(() => NotFound());
        }

        [HttpPost]
        public async Task<ActionResult<SongReadDto>> Post(SongCreateDto songCreateDto)
        {
            var song = _mapper.Map<Song>(songCreateDto);
            _unitOfWork.Songs.Create(song);
            await _unitOfWork.Save();
            var songReadDto = _mapper.Map<SongReadDto>(song);
            return await Task.Run(() => CreatedAtRoute(nameof(Get), new {Id = songReadDto.Id}, songReadDto)) ;
        }
    }
}