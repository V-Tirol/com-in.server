using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ForumContext _context;
        public DepartmentController(ForumContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> getDepartments()
        {
            var courses = await _context.Department.ToListAsync();
            return Ok(courses);
        }
    }
}
