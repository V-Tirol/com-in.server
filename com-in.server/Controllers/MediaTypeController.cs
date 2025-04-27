using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaTypeController : ControllerBase
    {
        private readonly ForumContext _context;

        public MediaTypeController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaType>>> GetTypes()
        {
            var types = await _context.MediaType
                .Where(c => c.isActive)
                .ToListAsync();

            return Ok(types);
        }
    }
}
