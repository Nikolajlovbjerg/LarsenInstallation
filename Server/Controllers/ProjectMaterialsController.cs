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
        private readonly IMaterialRepositorySQL _repo;

        public ProjectMaterialsController(IMaterialRepositorySQL repo)
        {
            _repo = repo;
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

                var materials = MaterialConverter.Convert(s);

                foreach (var m in materials)
                {
                    m.ProjectId = projectId;
                    _repo.Add(m);
                }
                return Ok($"Uploaded {materials.Count} materials.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing file: " + ex.Message);
            }
        }
    }
}