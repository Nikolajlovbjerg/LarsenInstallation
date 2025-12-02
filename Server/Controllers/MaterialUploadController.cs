using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.ExcelRepos;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/materialuploadexcel")]
    public class MaterialUploadController : ControllerBase
    {
        private readonly IMaterialExcelRepo MatExRepo;

        public MaterialUploadController(IMaterialExcelRepo MatExRepo)
        {
            this.MatExRepo = MatExRepo;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            else if (file.FileName.StartsWith("Mater"))
            {
                Stream s = new MemoryStream();
                file.CopyTo(s);

                List<ProjectMaterial> res = MaterialConverter.Convert(s);

                foreach (var row in res)
                {
                    MatExRepo.Add(row);
                }

                return Ok("Material uploaded");

            }
            return Ok();
        }
    }
}
