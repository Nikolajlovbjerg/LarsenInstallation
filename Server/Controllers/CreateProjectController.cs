using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Repositories.User;

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

        [HttpPost]
        public IActionResult Add(Project pro) //Fleksibel pakke. Bruges når man for en masse forskellige slags data.
        //Det er et interface der giver dig lov til at retunere hvad som helst så længe der er et gyldigt http svar
        {
            int newProjectId = crProj.Add(pro);
            return Ok(newProjectId); //Ok er en hjælpe metode der fortæller klienten det lykkedes og giver svaret
        }


    }
}
