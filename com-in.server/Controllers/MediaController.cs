using AutoMapper;
using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com_in.server.DTO;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IMapper _mapper;

        public MediaController(ForumContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Media
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
        {
            var media = await _context.Media
                .Include(c => c.Category)
                .Include(t => t.Type)
                .ToListAsync();

            var mediaDtos = _mapper.Map<List<MediaDto>>(media);
            return Ok(mediaDtos);
        }
    }
}
