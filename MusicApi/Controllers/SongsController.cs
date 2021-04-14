using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Repositories;

namespace MusicApi.Controllers
{
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISongRepository _songRepository;
    }
}