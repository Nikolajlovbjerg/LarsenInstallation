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

        // Repository som gemmer timer i databasen
        public ProjectHoursController(IHourRepository hourRepo)
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
                // Opretter en midlertidig hukommelse (stream)
                using Stream s = new MemoryStream();
                file.CopyTo(s); // Kopierer filens indhold ind i hukommelsen
                s.Position = 0; // starter læsningen fra begyndelsen af filen 


                //konverterer excel filen til material objekter 
                var hours = WorkerConverter.Convert(s);
                
                foreach (var h in hours)
                {
                    //sætter materialer til projekter
                    h.ProjectId = projectId;
                    // gemmer materialet til db 
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