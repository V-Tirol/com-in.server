using com_in.server.DTO;
using com_in.server.Helper;
using com_in.server.Models;
using com_in.server.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;


namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextMediaController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IWebHostEnvironment _environment;
        public TextMediaController(ForumContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// Get TextMedia Only
        /// </summary>
        /// <returns>Text Media</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextMedia>>> GetTextMedias()
        {
            var textMedias = await _context.TextMedia
                .Where(ia => !ia.isDeleted)
                .ToListAsync();

            return Ok(textMedias);
        }

        /// <summary>
        /// Insert / Update Media (text/multi) 
        /// File limit 100MB at the moment
        /// </summary>
        /// <param name="dto">Object dto</param>
        /// <returns>OperationResult(isSuccess, Message)</returns>
        [HttpPost]
        [RequestSizeLimit(500_000_000)] // 500MB for specific action
        [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)] // For form data
        public async Task<OperationResult> PostTextMedia([FromForm] TextMediaUploadDto dto)
        {
            string message = "";
            string? relativePath = "", fullPath = "";
            // check if dto has file or file has length
            if (dto.uploadedFile == null || dto.uploadedFile.Length == 0)
                return new OperationResult(false, "No File Uploaded");

            //check filetype
            var fileType = GetFileType(dto.uploadedFile);
            var uploadsDir = "";
            //get file extension
            var ext = Path.GetExtension(dto.uploadedFile.FileName).ToLowerInvariant();
            var filename = $"{Guid.NewGuid()}{ext}";

            //uploadsDir = Path.Combine(_environment.ContentRootPath, "Uploads/");


            //check for video,audio or not
            if (fileType == "video" || fileType == "audio")
                relativePath = Path.Combine("MultiMedia", filename);
            else
                relativePath = Path.Combine("TextMedia", filename);

            fullPath = Path.Combine(_environment.ContentRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool fileTypeMatch = true;
                int saveFileType = 0; // 1 = video, audio | 2 = article, research, picture
                if(dto.fileType == "research" || dto.fileType == "article" || dto.fileType == "picture")
                {
                    if(fileType == "video" || fileType == "audio")
                    {
                        fileTypeMatch = false;
                        message = "file type mismatch";
                        throw new ArgumentException(message);
                    }    
                    saveFileType = 2;
                }

                if(dto.fileType == "video")
                {
                    if(fileType != "video")
                    {
                        fileTypeMatch = false;
                        message = "file uploaded is not a video";
                        throw new ArgumentException(message);
                    }
                    saveFileType = 1;
                }

                if(dto.fileType == "audio")
                {
                    if(fileType != "audio")
                    {
                        fileTypeMatch = false;
                        message = "file uploaded is not an audio";
                        throw new ArgumentException(message);
                    }
                    saveFileType = 1;
                }


                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.uploadedFile.CopyToAsync(stream);
                }

                // check id > 0
                // if id > 0, then update, else insert
                if (dto.Id > 0)
                {
                    if (saveFileType == 1)
                    {
                        var existingMultimedia = await _context.MultiMedia.FirstOrDefaultAsync(m => m.Id == dto.Id);

                        if (existingMultimedia != null)
                        {
                            existingMultimedia.Title = dto.Title;
                            existingMultimedia.Author = dto.Author;
                            existingMultimedia.Date = dto.Date;
                            existingMultimedia.Description = dto.Description;
                            existingMultimedia.mediaType = dto.fileType;
                            existingMultimedia.uploaderId = dto.uploaderId;
                            existingMultimedia.departmentName = dto.category;
                            existingMultimedia.departmentType = dto.categoryType;
                            existingMultimedia.fileURL = relativePath;

                            // Only update duration if filePath is provided (new file uploaded)
                            if (!string.IsNullOrEmpty(fullPath))
                            {
                                existingMultimedia.duration = DurationGetter.GetVideoDuration(fullPath);
                            }

                        }
                    }
                    else
                    {
                        var existingTextmedia = await _context.TextMedia.FirstOrDefaultAsync(t => t.Id == dto.Id);
                        if (existingTextmedia != null)
                        {
                            existingTextmedia.Title = dto.Title;
                            existingTextmedia.Author = dto.Author;
                            existingTextmedia.Date = dto.Date;
                            existingTextmedia.Description = dto.Description;
                            existingTextmedia.mediaType = dto.fileType;
                            existingTextmedia.departmentName = dto.category;
                            existingTextmedia.departmentType = dto.categoryType;
                            existingTextmedia.fileURL = relativePath;
                        }
                    }

                    message = "File updated successfully!";
                }
                else
                {
                    if (saveFileType == 1)
                    {

                        var duration = DurationGetter.GetVideoDuration(fullPath);

                        var multiMedia = new MultiMedia
                        {
                            Title = dto.Title,
                            Author = dto.Author,
                            Date = dto.Date,
                            Description = dto.Description,
                            mediaType = dto.fileType,
                            uploaderId = dto.uploaderId,
                            duration = duration,
                            departmentName = dto.category,
                            departmentType = dto.categoryType,
                            isSuperAdminApprove = dto.isApproved,
                            fileURL = relativePath
                        };
                        _context.MultiMedia.Add(multiMedia);
                    }
                    else
                    {
                        var textMedia = new TextMedia
                        {
                            Title = dto.Title,
                            Author = dto.Author,
                            Date = dto.Date,
                            Description = dto.Description,
                            mediaType = dto.fileType,
                            uploaderId = dto.uploaderId,
                            departmentName = dto.category,
                            departmentType = dto.categoryType,
                            isSuperAdminApprove = dto.isApproved,
                            fileURL = relativePath
                        };
                        _context.TextMedia.Add(textMedia);
                    }
                    message = "File uploaded successfully!";
                }



                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


                return new OperationResult(true, message);
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(fullPath))
                {   
                    System.IO.File.Delete(fullPath);
                }

                await transaction.RollbackAsync();
                return new OperationResult(false, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationResult>> DeleteMedia(
            int? id,
            [FromQuery] string fileType
            )
        {
            if (id == null)
                return new OperationResult(false, "Media Not Found!");
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (fileType == "video" || fileType == "audio")
                {
                    var multimedia = await _context.MultiMedia.FirstOrDefaultAsync(m => m.Id == id);
                    if (multimedia == null)
                        return new OperationResult(false, "Multimedia Not Found!");

                    multimedia.isDeleted = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var textmedia = await _context.TextMedia.FirstOrDefaultAsync(t => t.Id == id);
                    if (textmedia == null)
                        return new OperationResult(false, "TextMedia Not Found!");

                    textmedia.isDeleted = true;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return new OperationResult(true, "Media Deleted");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new OperationResult(false, "There was an error deleting the file");
            }

        }

        public static string GetFileType(IFormFile file)
        {
            // First check the content type
            if (file.ContentType.StartsWith("video/"))
                return "video";
            if (file.ContentType.StartsWith("audio/"))
                return "audio";
            if (file.ContentType.StartsWith("image/"))
                return "image";

            // Then check extension if content type is generic
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm" };
            var audioExtensions = new[] { ".mp3", ".wav", ".aac", ".ogg", ".flac", ".wma" };

            if (videoExtensions.Contains(extension))
                return "video";
            if (audioExtensions.Contains(extension))
                return "audio";

            return "other";
        }



    }
}
