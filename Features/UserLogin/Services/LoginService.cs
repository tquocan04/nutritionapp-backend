using Features.UserLogin.Exceptions;
using Features.UserLogin.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Features.UserLogin.Services
{
    public class LoginService : ILoginService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IConfiguration _configuration;

        public LoginService(IRepositoryManager repositoryManager, IConfiguration configuration) 
        {
            _repositoryManager = repositoryManager;
            _configuration = configuration;
        }

        private List<Claim> GetClaim(LoginRequest login, string role, string id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, login.Username),
                new Claim(ClaimTypes.Role, role)
            };
            return claims;
        }

        private string GenerateToken(LoginRequest login, string role, string id)
        {
            var expiryyear = int.Parse(_configuration["Jwt:TokenExpiredInYear"]);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: GetClaim(login, role, id),
                expires: DateTime.Now.AddYears(expiryyear),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public async Task<(string, string)> Login(LoginRequest login)
        {
            var result = await _repositoryManager.Login.CheckLogin(login) ?? throw new InvalidLogin();

            var token = GenerateToken(login, result.RoleId, result.Id.ToString());

            return (token, result.RoleId);
        }
    }
}
