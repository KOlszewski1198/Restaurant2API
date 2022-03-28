using Microsoft.AspNetCore.Mvc;
using Restaurant2API.Models;
using Restaurant2API.Services;

namespace Restaurant2API.Controllers
{
    [Route("api/account")]
    [ApiController] //walidacja poprawności przesyłanego body do mapowania
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }

    }
}
