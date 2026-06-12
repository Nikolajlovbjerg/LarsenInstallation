using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.HourRepositories;
using Server.Service; 

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projecthours")]
    public class ProjectHoursController : ControllerBase
    {
        private readonly IHourRepository _hourRepo;
        private readonly ILogger<ProjectHoursController> _logger;

        // Repository som gemmer timer i databasen
        public ProjectHoursController(IHourRepository hourRepo, ILogger<ProjectHoursController> logger)
        {
            _hourRepo = hourRepo;
            _logger = logger;
        }
        
        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");
            try 
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0; 

                var hours = WorkerConverter.Convert(s);
                foreach (var h in hours)
                {
                    h.ProjectId = projectId;
                }

                // ONE call to the database instead of many
                _hourRepo.AddRange(hours); 

                return Ok($"Uploaded {hours.Count} hours.");
            }
            catch (Exception ex)
            {
                // Logger den fulde fejl på serveren, men sender kun en generisk besked til klienten
                _logger.LogError(ex, "Failed to process uploaded hours file for project {ProjectId}", projectId);
                return BadRequest("Could not process the uploaded hours file. Please check the file and try again.");
            }
        }
    }
}