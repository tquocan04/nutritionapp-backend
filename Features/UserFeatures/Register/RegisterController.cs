using Features.UserFeatures.DTOs;
using Features.UserFeatures.Reponses;
using Features.UserFeatures.Requests.Register;
using Microsoft.AspNetCore.Mvc;

namespace Features.UserFeatures.Register
{
    [Route("api/user")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public RegisterController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var result = await _serviceManager.UserService.Register(req);

            if (result == null)
            {
                return BadRequest("INVALID VALUE");
            }

            return Ok(new RegisterResponse<NewUserResponseDTO>
            {
                Message = "Register successful.",
                Data = result
            });
        }
    }
}
