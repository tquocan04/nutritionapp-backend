using Features.UserLogin.Requests;
using Features.UserLogin.Response;
using Microsoft.AspNetCore.Mvc;

namespace Features.UserLogin
{
    [Route("api/user")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;

        public LoginController(IRepositoryManager repositoryManager,
            IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
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
                Role = result.Item2,
                Token = $"Bearer {result.Item1}"
            }
                );
        }
    }
}
