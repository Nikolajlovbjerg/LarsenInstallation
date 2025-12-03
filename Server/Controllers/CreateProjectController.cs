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
        public void Add(Project pro)
        {
            crProj.Add(pro);
        }


    }
}
