using com_in.server.DTO;
using com_in.server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllMediaController : ControllerBase
    {
        private readonly ForumContext _context;

        public AllMediaController(ForumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllMediaDto>>> GetAllMedia()
        {

            var textmedia = await _context.TextMedia
                .Select(item => new AllMediaDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Author = item.Author,
                    Date = item.Date,
                    Description = item.Description,
                    mediaType = item.mediaType,
                    departmentName = item.departmentName,
                    departmentType = item.departmentType,
                    uploaderId = item.uploaderId,
                    isSuperAdminApprove = item.isSuperAdminApprove,
                    isDeleted = item.isDeleted
                }).ToListAsync();

            var multiMedia = await _context.MultiMedia
                .Select(item => new AllMediaDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Author = item.Author,
                    Date = item.Date,
                    Description = item.Description,
                    Duration = item.duration,
                    mediaType = item.mediaType,
                    departmentName = item.departmentName,
                    departmentType = item.departmentType,
                    uploaderId = item.uploaderId,
                    isSuperAdminApprove = item.isSuperAdminApprove,
                    isDeleted = item.isDeleted
                }).ToListAsync();


            var textMediaWithIterator = textmedia
                .Select((item, index) => new AllMediaDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Author = item.Author,
                    Date = item.Date,
                    Description = item.Description,
                    mediaType = item.mediaType,
                    departmentName = item.departmentName,
                    departmentType = item.departmentType,
                    uploaderId = item.uploaderId,
                    isSuperAdminApprove = item.isSuperAdminApprove,
                    isDeleted = item.isDeleted,
                    iterator = index + 1
                });

            var multiMediaWithIterator = multiMedia
                .Select((item, index) => new AllMediaDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Author = item.Author,
                    Date = item.Date,
                    Description = item.Description,
                    Duration = item.Duration,
                    mediaType = item.mediaType,
                    departmentName = item.departmentName,
                    departmentType = item.departmentType,
                    uploaderId = item.uploaderId,
                    isSuperAdminApprove = item.isSuperAdminApprove,
                    isDeleted = item.isDeleted,
                    iterator = index + 1 + textmedia.Count()
                });

            var combined = textMediaWithIterator.Concat(multiMediaWithIterator).ToList();

            return Ok(combined);
        }
    }
}
