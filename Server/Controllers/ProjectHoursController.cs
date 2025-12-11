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
        private readonly IHourRepositorySQL _hourRepo;

        public ProjectHoursController(IHourRepositorySQL hourRepo)
        {
            _hourRepo = hourRepo;
        }

        /*[HttpGet]
        public IEnumerable<Calculation> GetOverview()
        {
            return _hourRepo.GetTotalHoursGroupedByType();
        }*/
        
        [HttpPost("upload")]
        //
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
                    _hourRepo.Add(h);
                }
                return Ok($"Uploaded {hours.Count} hours.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing file: " + ex.Message);
            }
        }
    }
}