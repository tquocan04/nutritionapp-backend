using AutoMapper;
using Domains;
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
        private readonly IMapper _mapper;
        private const float caloPerKg = 7700;

        public LoginService(IRepositoryManager repositoryManager, IConfiguration configuration, IMapper mapper) 
        {
            _repositoryManager = repositoryManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        private List<Claim> GetClaim(LoginRequest login, string id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, login.Username),
            };
            return claims;
        }

        private string GenerateToken(LoginRequest login, string id)
        {
            var expiryyear = int.Parse(_configuration["Jwt:TokenExpiredInYear"]);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: GetClaim(login, id),
                expires: DateTime.Now.AddYears(expiryyear),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public async Task<(string, User)> Login(LoginRequest login)
        {
            var result = await _repositoryManager.Login.CheckLogin(login) ?? throw new InvalidLogin();

            var token = GenerateToken(login, result.Id.ToString());

            return (token, result);
        }

        public async Task<IList<string>> InformationDetail(InformationDetailRequest req, Guid id)
        {
            var user = await _repositoryManager.Login.Getuser(id) ?? throw new UserNotFoundException(id);

            _mapper.Map(req, user);
            
            float bmr = (float)(10 * req.Weight + 6.25 * req.Height - 5 * req.Age);

            if (req.Gender.Equals("male", StringComparison.CurrentCultureIgnoreCase))
                user.BMR = bmr + 5;
            else
                user.BMR = bmr - 161;

            user.TDEE = user.BMR * user.R;

            _repositoryManager.User.Update(user);

            float diff = Math.Abs((float)(req.Weight - req.TargetWeight));

            IList<string> timeList = [];

            timeList.Add(Math.Round((diff * caloPerKg / 200)).ToString() + " days");
            timeList.Add(Math.Round((diff * caloPerKg / 500)).ToString() + " days");
            
            await _repositoryManager.SaveAsync();

            return timeList;
        }
    }
}
