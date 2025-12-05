using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Service;
using System.IO;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/createproject")]
    public class CreateProjectController : ControllerBase
    {

        private ICreateProjectRepo crProj;

        public CreateProjectController(ICreateProjectRepo crProj)
        {
            this.crProj = crProj;
        }

        [HttpGet] 
        public ActionResult<IEnumerable<Project>> GetAllProjects()
        {
            var projects = crProj.GetAllProjects();
            return Ok(projects);
        }
            
        [HttpPost]
        public IActionResult Add(Project pro) //Fleksibel pakke. Bruges når man for en masse forskellige slags data.
        //Det er et interface der giver dig lov til at retunere hvad som helst så længe der er et gyldigt http svar
        {
            int newProjectId = crProj.Add(pro);
            return Ok(newProjectId); //Ok er en hjælpe metode der fortæller klienten det lykkedes og giver svaret
        }

        [HttpGet("{id}")]
        public ActionResult<Calculation> GetProjectDetails(int id)
        {
                var result = crProj.GetProjectDetails(id);
                if (result == null) return NotFound("Project not found");
                {
                    return Ok(result);
                }
        }
        
        [HttpPost("uploadhours")]
        public IActionResult UploadHours(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
            
            // Tjekker om filnavnet starter med "case" (som i den gamle controller)
            if (file.FileName.StartsWith("case"))
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                
                List<ProjectHour> res = WorkerConverter.Convert(s);
                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    crProj.AddHour(row); // Bruger AddHour fra interfacet
                }
                return Ok("Worker hours uploaded for project " + projectId);
            }
            return BadRequest("Invalid file name or format");
        }
        
        [HttpPost("uploadmaterials")]
        public IActionResult UploadMaterials(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
            
            // Tjekker om filnavnet starter med "Mater" (som i den gamle controller)
            if (file.FileName.StartsWith("ordrematerialer"))
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                
                List<ProjectMaterial> res = MaterialConverter.Convert(s);
                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    crProj.AddMaterials(row); // Bruger AddMaterials fra interfacet
                }
                return Ok("Materials uploaded for project " + projectId);
            }
            return BadRequest("Invalid file name or format");
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Project pro)
        {
            // Tjekker om ID i URL'en matcher ID i objektet
            if (id != pro.ProjectId)
            {
                return BadRequest("ID mismatch");
            }

            try 
            {
                // Vi kalder Update metoden i dit repository (se trin 2)
                crProj.Update(pro);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server fejl: " + ex.Message);
            }
        }
        
    }
}
