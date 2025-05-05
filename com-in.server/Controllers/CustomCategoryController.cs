using com_in.server.DTO;
using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomCategoryController : ControllerBase
    {
        private readonly ForumContext _context;

        public CustomCategoryController(ForumContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDepartmentDto>>> GetCustomCategoriesDropDown()
        {
            var courses = await _context.Courses
                .Select(c => new CourseDepartmentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = "Student"

                }).ToListAsync();



            var departments = await _context.Department
                .Select(c => new CourseDepartmentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = "Faculty"
                }).ToListAsync();
                

            var coursesWithUniqueId = courses
                        .Select((c, index) => new CourseDepartmentDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = "Student",
                            UniqueId = index + 1

                        });

            var departmentsWithUniqueId = departments
                        .Select((c, index) => new CourseDepartmentDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = "Faculty",
                            UniqueId = index + 1 + courses.Count()
                        });

            var combinedResult = coursesWithUniqueId.Concat(departmentsWithUniqueId).ToList();

            return Ok(combinedResult);
        }
    }
}
