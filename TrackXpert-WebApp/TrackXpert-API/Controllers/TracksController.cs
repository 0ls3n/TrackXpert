using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackXpert_API.Data;
using TrackXpert_ClassLibrary.Models.TrackData;

namespace TrackXpert_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        IConfiguration _config;

        private readonly DataContext _context;

        public TracksController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            var maxFileSize = 250 * 1024 * 1024; // Max size is 250 MB
            var fileStoragePath = _config.GetValue<string>("FileStorage")!;
            string relativeFilePath = string.Empty;


            var errors = new List<string>();

            try
            {
                if (file.Length > maxFileSize)
                {
                    errors.Add($"File: {file.FileName} exceeds the maximum allowed size of {maxFileSize / 1024 / 1024} MB.");
                }

                string newFileName = Path.ChangeExtension(
                    Path.GetRandomFileName(),
                    Path.GetExtension(file.FileName));

                relativeFilePath = Path.Combine("tcorey", newFileName);

                string path = Path.Combine(fileStoragePath, "tcorey", newFileName);
                Directory.CreateDirectory(Path.Combine(fileStoragePath, "tcorey"));

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                errors.Add($"File: {file.FileName} Error: {ex.Message}");
                relativeFilePath = "";
            }


            if (errors.Count > 0)
            {
                return BadRequest(new { Message = "Some files failed to upload.", Errors = errors });
            }

            return Ok(new { Message = "Files uploaded successfully!", FilePath = relativeFilePath });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrack([FromBody] Track track)
        {
            try
            {
                await _context.Tracks!.AddAsync(track);
                await _context.SaveChangesAsync();

				return Ok(track);
			} catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Track> tracks = await _context.Tracks!.ToListAsync();
                return Ok(tracks);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
