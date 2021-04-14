using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using MusicApi.DTOs;
using MusicApi.Repositories;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SongsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongReadDto>>> Get()
        {
            var songs = await _unitOfWork.Songs.FindAll();
            if (songs.Any())
            {
                var songReadDto = _mapper.Map<IEnumerable<SongReadDto>>(songs);
                return Ok(songReadDto);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongReadDto>> Get(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song != null)
            {
                var songReadDto = _mapper.Map<SongReadDto>(song);
                return Ok(songReadDto);
            }

            return NotFound();
        }
    }
}