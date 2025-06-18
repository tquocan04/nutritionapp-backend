using AutoMapper;
using Datas;
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
                new Claim(ClaimTypes.Email, login.Email),
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

        public async Task UpdateTimeAsync(int time, Guid id)
        {
            var user = await _repositoryManager.Login.Getuser(id) ?? throw new UserNotFoundException(id);

            user.Time = time;
            user.IsActive = true;

            _repositoryManager.User.Update(user);

            DateOnly date = DateOnly.FromDateTime(DateTime.Today);

            float caloPerDay = 7700 / user.Time;

            float calo = (float)user.TDEE; // calo
            float fat = (float)user.Weight; // gram
            float protein = (float)user.Weight; // gram

            if (user.Weight < user.TargetWeight) // tang can
            {
                calo += caloPerDay;
                fat *= 0.33f;
            }
            else if (user.Weight > user.TargetWeight) // giam can
            {
                calo -= caloPerDay;
                fat *= 0.23f;
                protein *= 1.1f;
            }
            else // giu can
            {
                fat *= 0.33f;
            }

            Guid dailyId = Guid.NewGuid();

            var breakfast = new Breakfast
            {
                TotalCalories = 0,
                TotalCarbs = 0,
                TotalFats = 0,
                TotalProteins = 0,
                DailyPlan_id = dailyId,
            };

            var lunch = new Lunch
            {
                TotalCalories = 0,
                TotalCarbs = 0,
                TotalFats = 0,
                TotalProteins = 0,
                DailyPlan_id = dailyId,
            };

            var dinner = new Dinner
            {
                TotalCalories = 0,
                TotalCarbs = 0,
                TotalFats = 0,
                TotalProteins = 0,
                DailyPlan_id = dailyId,
            };

            // Thêm vào context
            var breakfastTask = _repositoryManager.Login.AddNewBreakfastAsync(breakfast);
            var lunchTask = _repositoryManager.Login.AddNewLunchAsync(lunch);
            var dinnerTask = _repositoryManager.Login.AddNewDinnerAsync(dinner);
            await Task.WhenAll(breakfastTask, lunchTask, dinnerTask);

            // DailyPlan
            var dailyPlan = new DailyPlan
            {
                Id = dailyId,
                User_id = user.Id,
                Breakfast_id = breakfast.Id,
                Lunch_id = lunch.Id,
                Dinner_id = dinner.Id,
                TargetCalories = (float)Math.Floor((decimal)calo),
                TargetFats = (float)Math.Floor((decimal)fat), // gram
                TargetProteins = (float)Math.Floor((decimal)protein), // gram
                TargetCarbs = (float)Math.Floor((decimal)(calo - protein * 4 - fat * 9) / 4),
                TotalCalories = 0,
                TotalFats = 0,
                TotalProteins = 0,
                Date = date
            };

            await _repositoryManager.Login.AddNewDailyPlanAsync(dailyPlan);

            await _repositoryManager.SaveAsync();
        }
    }
}
