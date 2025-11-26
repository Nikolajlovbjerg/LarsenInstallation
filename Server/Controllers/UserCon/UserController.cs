using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.User;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {

        private ICreateUserRepoSQL userRepo;

        public UserController(ICreateUserRepoSQL userRepo)
        {
            this.userRepo = userRepo;
        }

        [HttpGet]
        public IEnumerable<Users> Get()
        {
            return userRepo.GetAll();
        }

        [HttpPost]
        public void Add(Users user)
        {
            userRepo.Add(user);
        }
        
        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] Login dto)
        {
            var user = userRepo.ValidateUser(dto.UserName, dto.Password); // vi tilføjer denne metode til repo
            if (user == null)
                return Unauthorized(); // 401
            return Ok(user);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public void Delete(int id)
        {
            userRepo.Delete(id);
        }

        [HttpDelete]
        [Route("delete")]
        public void DeleteByQuery([FromQuery] int id)
        {
            userRepo.Delete(id);
        }



    }
}