using com_in.server.DTO;
using com_in.server.Models;
using com_in.server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IWebHostEnvironment _environment;
        public AnnouncementController(ForumContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcements>>> GetAnnouncements()
        {
            var announcements = await _context.Announcements
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    FormattedDate = a.PublishDateAndTime.ToString("yyyy-MM-dd hh:mmtt"),
                    a.Content,
                    a.Category,
                    a.Status,
                    imgURL = $"/uploads/{a.imageURL}",
                    a.PublishedById
                })
                .ToListAsync();

            return Ok(announcements);
        }

        [HttpPost]
        public async Task<ActionResult<OperationResult>> AddAnnouncement(AnnouncementInsertDto dto)
        {

            string? relativePath = "";
            string? fullPath = "";
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (dto.image == null || dto.image.Length == 0)
                    return new OperationResult(false, "No file uploaded");

                //(relativePath, fullPath) = await _uploadService.fileUpload(dto.image);

                var ext = Path.GetExtension(dto.image.FileName).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{ext}";

                relativePath = Path.Combine("Announcements", fileName);
                



                if (dto.Id > 0)
                {
                    var existingAnnouncement = await _context.Announcements.FirstOrDefaultAsync(c => c.Id == dto.Id);
                    var currentImageName = existingAnnouncement.imageURL.Split('\\')[1];
                    var newFileName = dto.image.FileName;

                    if (currentImageName == newFileName)
                        relativePath = Path.Combine("Announcements", currentImageName);

                    if (existingAnnouncement != null)
                    {
                        existingAnnouncement.Title = dto.Title;
                        existingAnnouncement.Content = dto.Excerpt;
                        existingAnnouncement.Category = dto.Category;
                        existingAnnouncement.imageURL = relativePath;

                    }
                }
                else
                {
                    var announcement = new Announcements
                    {
                        Title = dto.Title,
                        Content = dto.Excerpt,
                        Category = dto.Category,
                        PublishDateAndTime = DateTime.Now,
                        Status = dto.Status,
                        PublishedById = dto.PublishedById,
                        imageURL = relativePath
                    };

                    _context.Announcements.Add(announcement);
                }

                fullPath = Path.Combine(_environment.ContentRootPath, "uploads", relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.image.CopyToAsync(stream);
                }


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new OperationResult(true, "Announcement Uploaded!"));
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                await transaction.RollbackAsync();
                return BadRequest(new OperationResult(false, "Failed to upload file(s)"));
            }

        }

        [HttpGet("updateStatus")]
        public async Task<ActionResult<OperationResult>> UpdatePublish(int id, string status)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var announcement = await _context.Announcements.SingleOrDefaultAsync(i => i.Id == id);
                if (announcement == null)
                    return BadRequest(new OperationResult(false, "Announcement not Found!"));

                announcement.Status = status;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new OperationResult(true, "Success!"));
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest(new OperationResult(false, "There was an error updating announcement."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAnnouncement(int? id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var announcements = await _context.Announcements.FindAsync(id);
                if (announcements != null)
                {
                    _context.Announcements.Remove(announcements);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "uploads", announcements.imageURL)))
                {
                    System.IO.File.Delete(Path.Combine(_environment.ContentRootPath, "uploads", announcements.imageURL));
                }

                return Ok(new OperationResult(true, "deleted"));
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest(new OperationResult(false, "Error deleting announcement"));
            }
        }

    }
}
