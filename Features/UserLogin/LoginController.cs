using Domains;
using Features.UserLogin.Requests;
using Features.UserLogin.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Features.UserLogin
{
    [Route("api/user")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IRepositoryManager _repositoryManager;

        public LoginController(IServiceManager serviceManager, IRepositoryManager repositoryManager)
        {
            _serviceManager = serviceManager;
            _repositoryManager = repositoryManager;
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
                Status = result.Item2.IsActive,
                Name = result.Item2.Name,
            }
            );
        }
        
        [HttpPost("information")]
        [Authorize]
        public async Task<IActionResult> InformationDetail([FromBody] InformationDetailRequest req)
        {
            Guid id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _serviceManager.LoginService.InformationDetail(req, id);

            return Ok(new MessageResponse<IList<string>>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
