using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackXpert_API.Models;

namespace TrackXpert_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult UploadTrack([FromBody] Track track)
        {
            return Ok(track);
        }
    }
}
