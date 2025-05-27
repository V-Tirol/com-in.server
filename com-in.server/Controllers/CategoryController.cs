using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ForumContext _context;

        public CategoryController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.isActive)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet()]
        [Route("AnnouncementCategory")]
        public async Task<ActionResult<IEnumerable<AnnouncementCategories>>> GetAnnouncemntCategories()
        {
            var announcementCategories = await _context.AnnouncementCategories
                .Where(c => c.isActive)
                .ToListAsync();

            return Ok(announcementCategories);
        }
        


    }
}
