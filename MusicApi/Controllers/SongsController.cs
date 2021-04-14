using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicApi.Data;
using MusicApi.DTOs;

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
                return Ok(songReadDto);
            }

            _logger.LogWarning("No Entities Found");
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongReadDto>> Get(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song != null)
            {
                var songReadDto = _mapper.Map<SongReadDto>(song);
                _logger.LogInformation("Entity found");
                return Ok(songReadDto);
            }

            _logger.LogWarning("Wrong entity Id.");
            return NotFound();
        }
    }
}