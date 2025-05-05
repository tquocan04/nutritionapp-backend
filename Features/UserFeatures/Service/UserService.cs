using AutoMapper;
using Domains;
using Features.UserFeatures.DTOs;
using Features.UserFeatures.Exceptions;
using Features.UserFeatures.Requests.Register;

namespace Features.UserFeatures.Service
{
    public class UserService(IRepositoryManager repositoryManager, IMapper mapper) : IUserService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;
        
        public async Task<NewUserResponseDTO?> Register(RegisterRequest req)
        {
            if (await _repositoryManager.User.CheckEmailExist(req))
                throw new EmailBadRequestException(req.Email);

            User newUser = _mapper.Map<User>(req);
            await _repositoryManager.User.Create(newUser);

            // dailyplan
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (newUser.Time == 0 || newUser.TDEE == null || newUser.Weight == null)
            {
                return null;
            }

            float caloPerDay = 7700 / newUser.Time;

            float calo = (float)newUser.TDEE; // calo
            float fat = (float)newUser.Weight; // gram
            float protein = (float)newUser.Weight; // gram

            if (newUser.Weight < newUser.TargetWeight) // tang can
            {
                calo += caloPerDay;
                fat *= 0.33f;
            }
            else if (newUser.Weight > newUser.TargetWeight) // giam can
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
            var breakfastTask = _repositoryManager.User.AddNewBreakfastAsync(breakfast);
            var lunchTask = _repositoryManager.User.AddNewLunchAsync(lunch);
            var dinnerTask = _repositoryManager.User.AddNewDinnerAsync(dinner);
            await Task.WhenAll(breakfastTask, lunchTask, dinnerTask);

            // DailyPlan
            var dailyPlan = new DailyPlan
            {
                Id = dailyId,
                User_id = newUser.Id,
                Breakfast_id = breakfast.Id,
                Lunch_id = lunch.Id,
                Dinner_id = dinner.Id,
                TargetCalories = calo,
                TargetFats = fat, // gram
                TargetProteins = protein, // gram
                TargetCarbs = (calo - protein * 4 - fat * 9) / 4,
                TotalCalories = 0,
                TotalFats = 0,
                TotalProteins = 0,
                Date = today
            };

            await _repositoryManager.User.AddNewDailyPlanAsync(dailyPlan);

            await _repositoryManager.SaveAsync();
            return _mapper.Map<NewUserResponseDTO>(newUser);
        }
    }
}
