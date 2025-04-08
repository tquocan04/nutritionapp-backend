using Features.UserLogin.Requests;
using Features.UserLogin.Response;
using Features.UserLogin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Features.UserLogin
{
    [Route("api/user")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public LoginController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceManager.LoginService.Login(login);

            return Ok(new LoginResponse
            {
                Token = $"Bearer {result.Item1}",
                Image = result.Item2.Image,
                Status = result.Item2.IsActive
            }
            );
        }
    }
}
