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
        public IActionResult Upload(IFormFile? file)
        {
            if (file == null || file.Length == 0) 
                return BadRequest("No file uploaded");

            else if (file.FileName.StartsWith("Work"))
            {
                    Stream s = new MemoryStream();
                    file.CopyTo(s);

                    List<ProjectHour> res = WorkerConverter.Convert(s);

                foreach (var row in res)
                {
                    exRepo.Add(row);
                }

                return Ok("worker uploaded");

                }
            return Ok();
        }
    }
}
