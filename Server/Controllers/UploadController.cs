using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.ExcelRepos;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/uploadexcel")]
    public class UploadController : ControllerBase
    {
        private readonly IExcelRepo exRepo;

        public UploadController(IExcelRepo exRepo)
        {
            this.exRepo = exRepo;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile? file, int projectId) 
            //ProjectId parameteren er der for at vi kan modtage id udefra
        {
            if (file == null || file.Length == 0) 
                return BadRequest("No file uploaded");

            else if (file.FileName.StartsWith("Work"))
            {
                    Stream s = new MemoryStream();
                    file.CopyTo(s);
                    s.Position = 0; //Går tilbage til starten så vi kan læse dataen. Fordi efter filen er indlæst vil den være i sidste kolonne

                    List<ProjectHour> res = WorkerConverter.Convert(s);

                foreach (var row in res)
                {
                    row.ProjectId = projectId; //Her for id værdien
                    exRepo.Add(row);
                }

                return Ok("worker uploaded" + projectId);

                }
            return Ok();
        }
    }
}
