using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiMediaController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IWebHostEnvironment _environment;

        public MultiMediaController(ForumContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// Get TextMedia Only
        /// </summary>
        /// <returns>Text Media</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MultiMedia>>> GetMultiMedia()
        {
            var multiMedia = await _context.MultiMedia
                .Where(ia => !ia.isDeleted)
                .ToListAsync();

            return Ok(multiMedia);
        }
    }
}
