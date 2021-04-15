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
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArtistController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistReadDto>>> Get()
        {
            var artistList = await _unitOfWork.Artists.FindAll();
            if (artistList.Any())
            {
                var artistReadDtoList = _mapper.Map<IEnumerable<ArtistReadDto>>(artistList);
                return Ok(artistReadDtoList);
            }

            return await Task.Run(NoContent);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongReadDto>> Get(int id)
        {
            var artist = await _unitOfWork.Artists.Find(id);
            if (artist != null)
            {
                var artistReadDto = _mapper.Map<ArtistReadDto>(artist);
                return await Task.Run(() => Ok(artistReadDto));
            }

            return await Task.Run(NotFound);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistReadDto>> Post(ArtistCreateDto artistCreateDto)
        {
            var artist = _mapper.Map<Artist>(artistCreateDto);
            await _unitOfWork.Artists.Create(artist);
            await _unitOfWork.Save();

            var artistReadDto = _mapper.Map<ArtistReadDto>(artist);
            return await Task.Run(() => CreatedAtAction(nameof(Get), new {Id = artistReadDto.Id}, artistReadDto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistReadDto>> Put(int id, ArtistUpdateDto artistUpdateDto)
        {
            var artist = await _unitOfWork.Artists.Find(id);
            if (artist != null)
            {
                _mapper.Map(artistUpdateDto, artist);
                await _unitOfWork.Artists.Update(artist);
                await _unitOfWork.Save();
                var artistReadDto = _mapper.Map<ArtistReadDto>(artist);
                return await Task.Run(() => Ok(artistReadDto));
            }

            return await Task.Run(NotFound);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ArtistReadDto>> Patch(int id, JsonPatchDocument<ArtistUpdateDto> patchDocument)
        {
            var artist =  await _unitOfWork.Artists.Find(id);
            if (artist == null) 
                return await Task.Run(NotFound);
            
            var artistToPatch = _mapper.Map<ArtistUpdateDto>(artist);
            patchDocument.ApplyTo(artistToPatch, ModelState);
            if (!TryValidateModel(artistToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(artistToPatch, artist);
                    
            var artistReadDto = _mapper.Map<ArtistReadDto>(artist);
            return await Task.Run(() => Ok(artistReadDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var artist = await _unitOfWork.Artists.Find(id);
            if (artist == null)
                return await Task.Run(NotFound);
            await _unitOfWork.Artists.Delete(artist);
            await _unitOfWork.Save();
            return await Task.Run(NoContent);
        }
    }
}