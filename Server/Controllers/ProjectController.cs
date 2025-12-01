using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using System.Text.Json;
using Server.Repositories.Project;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly ExcelReaderService _excelService;

        public ProjectController(IProjectRepository repo, ExcelReaderService excelService)
        {
            _repo = repo;
            _excelService = excelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject()
        {
            // Vi læser form data manuelt for at håndtere JSON + filer blandet sammen
            var form = await Request.ReadFormAsync();

            // 1. Hent Projekt JSON
            if (!form.TryGetValue("project", out var projectString))
                return BadRequest("Mangler projekt data");

            var project = JsonSerializer.Deserialize<Project>(projectString.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (project == null) return BadRequest("Kunne ikke læse projekt data");

            // Lister til data
            var hours = new List<ProjectHour>();
            var materials = new List<ProjectMaterial>();

            // 2. Behandl Time filer
            foreach (var file in form.Files.Where(f => f.Name == "timeFile"))
            {
                using var stream = file.OpenReadStream();
                var fileHours = _excelService.ParseHours(stream);
                hours.AddRange(fileHours);
            }

            // 3. Behandl Materiale filer
            foreach (var file in form.Files.Where(f => f.Name == "materialFile"))
            {
                using var stream = file.OpenReadStream();
                var fileMaterials = _excelService.ParseMaterials(stream);
                materials.AddRange(fileMaterials);
            }

            // 4. Gem i databasen
            try
            {
                int newId = _repo.CreateProject(project, hours, materials);
                return Ok(new { ProjectId = newId, Message = "Projekt oprettet success" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Fejl ved lagring i database: " + ex.Message);
            }
        }
    }
}