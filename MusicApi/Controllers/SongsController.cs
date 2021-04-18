using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicApi.Data;
using MusicApi.DTOs;
using MusicApi.Models;
using MusicApi.Pagination;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUriService _uriService;

        public SongsController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<SongsController> logger, IUriService uriService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _uriService = uriService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DTOs.SongReadDto>>> Get([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            
            var songs = await _unitOfWork.Songs.FindWithPagingFilter(validFilter);
            if (songs.Any())
            {
                _logger.LogInformation("Entities Found");
                var songReadDto = _mapper.Map<IEnumerable<DTOs.SongReadDto>>(songs);
                var route = Request.Path.Value;
                var totalRecords = await _unitOfWork.Songs.Count();
                var pagedResponse = PaginationHelper.CreatePagedReponse<DTOs.SongReadDto>(songReadDto, validFilter, totalRecords, _uriService, route);
                return await Task.Run(() => Ok(pagedResponse));
            }

            _logger.LogWarning("No Entities Found");
            return await Task.Run(NotFound);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DTOs.SongReadDto>> Get(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song != null)
            {
                var songReadDto = _mapper.Map<DTOs.SongReadDto>(song);
                _logger.LogInformation("Entity found");
                return await Task.Run(() => Ok(new Response<DTOs.SongReadDto>(songReadDto)));
            }

            _logger.LogWarning("Wrong entity Id.");
            return await Task.Run(NotFound);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DTOs.SongReadDto>> Post(SongCreateDto songCreateDto)
        {
            var song = _mapper.Map<Song>(songCreateDto);
            await _unitOfWork.Songs.Create(song);
            await _unitOfWork.Save();
            var songReadDto = _mapper.Map<DTOs.SongReadDto>(song);
            _logger.LogInformation("Entity Created");
            return await Task.Run(() => CreatedAtAction(nameof(Get), new {Id = songReadDto.Id}, songReadDto));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DTOs.SongReadDto>> Update(int id, SongUpdateDto songUpdateDto)
        {
            var song = await _unitOfWork.Songs.Find(id);

            if (song == null)
            {
                _logger.LogWarning("Entity not found.");
                return await Task.Run(NotFound);
            }

            _mapper.Map(songUpdateDto, song);

            await _unitOfWork.Songs.Update(song);
            await _unitOfWork.Save();

            var songReadDto = _mapper.Map<DTOs.SongReadDto>(song);
            _logger.LogInformation("Entity variables changed successfully");
            return await Task.Run(() => Ok(songReadDto));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DTOs.SongReadDto>> Patch(int id, JsonPatchDocument<SongUpdateDto> patchDocument)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song == null)
            {
                _logger.LogWarning("Entity not found.");
                return await Task.Run(NotFound);
            }

            var songToPatch = _mapper.Map<SongUpdateDto>(song);
            patchDocument.ApplyTo(songToPatch, ModelState);
            if (!TryValidateModel(songToPatch))
            {
                _logger.LogWarning("Validation model problem.");
                return ValidationProblem(ModelState);
            }

            _mapper.Map(songToPatch, song);

            await _unitOfWork.Songs.Update(song);
            await _unitOfWork.Save();

            var songReadDto = _mapper.Map<DTOs.SongReadDto>(song);
            _logger.LogInformation("Entity parameters changed successfully");
            return await Task.Run((() => Ok(songReadDto)));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            var song = await _unitOfWork.Songs.Find(id);
            if (song == null)
            {
                _logger.LogWarning("Entity not found.");
                return await Task.Run(NotFound);
            }
            await _unitOfWork.Songs.Delete(song);
            await _unitOfWork.Save();
            _logger.LogInformation("Deleted successfully.");
            return await Task.Run(NoContent);
        }
    }
}