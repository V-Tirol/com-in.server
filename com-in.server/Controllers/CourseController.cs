using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ForumContext _context;
        public CourseController(ForumContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> getCourses()
        {
            var courses = await _context.Courses.ToListAsync();

            return Ok(courses);
        }
    }
}
