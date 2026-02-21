using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.MaterialRepositories;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projectmaterials")]
    public class ProjectMaterialsController : ControllerBase
    {
        // Repository som gemmer materialer i databasen
        private readonly IMaterialRepository _repo;

        // Repository som gemmer materialer i databasen
        public ProjectMaterialsController(IMaterialRepository repo)
        {
            _repo = repo;
        }


        // Et endpoint der bruges n�r man uploader en fil

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file selected.");
            try
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0; 

                // Convert Excel rows to objects
                var materials = MaterialConverter.Convert(s);
        
                // Assign ProjectId to all items
                foreach (var m in materials)
                {
                    m.ProjectId = projectId;
                }

                // Bulk Save
                _repo.AddRange(materials);

                return Ok($"Successfully processed {materials.Count} materials.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing material file: " + ex.Message);
            }
        }
    }
}