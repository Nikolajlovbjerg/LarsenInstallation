/*
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Repositories.ExcelRepos;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/materialuploadexcel")]
    public class MaterialUploadController : ControllerBase
    {
        private readonly ICreateProjectRepo MatExRepo;

        public MaterialUploadController(ICreateProjectRepo MatExRepo)
        {
            this.MatExRepo = MatExRepo;
        }

        [HttpPost]
        public IActionResult UploadMaterial(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            else if (file.FileName.StartsWith("Mater"))
            {
                Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                
                List<ProjectMaterial> res = MaterialConverter.Convert(s);

                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    MatExRepo.Add(row);
                }

                return Ok("Material uploaded" + projectId);

            }
            return Ok();
        }
    }
}
*/
