using AutoMapper;
using Features.DailyJobs.DTOs;
using Features.DailyJobs.Exceptions;
using Features.DailyJobs.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Features.DailyJobs.Services
{
    public class DailyPlanService(IRepositoryManager manager, IMapper mapper) 
        : IDailyPlanService
    {
        private readonly IRepositoryManager _manager = manager;
        private readonly IMapper _mapper = mapper;

        public async Task<DailyPlanDTO> GetDailyPlanAsync(Guid userId, DateRequest req)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            DateOnly dateRequest = new(req.Year, req.Month, req.Date);

            if (dateRequest > today)
            {
                throw new DateBadRequestException();
            }

            var result = await _manager.DailyPlan.GetDailyPlanAsync(userId, dateRequest, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, dateRequest);

            return _mapper.Map<DailyPlanDTO>(result);
        }

        public async Task<WeeklyProgressDTO> GetCaloriesInWeekAsync(Guid userId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            // Tìm ngày đầu tuần (giả sử tuần bắt đầu từ thứ Hai)
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            var dailyList = await _manager.DailyPlan.GetDailyPlanInWeekAsync(userId, startOfWeek,
                endOfWeek, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, startOfWeek);

            //var caloriesList = new List<CaloriesDTO>();

            var daily = await _manager.DailyPlan.GetDailyPlanAsync(userId, today, false);

            if (daily == null)
                return null;

            CaloriesDTO caloriesDTO = new()
            {
                Current = daily.TotalCalories,
                Target = daily.TargetCalories
            };

            Macros macrosCarbs = new()
            {
                Name = "Carbs",
                Value = daily.TotalCarbs
            };

            Macros macrosPro = new()
            {
                Name = "Protein",
                Value = daily.TotalProteins
            };

            Macros macrosFat = new()
            {
                Name = "Fat",
                Value = daily.TotalFats
            };

            IList<Macros> macros = [macrosCarbs, macrosPro, macrosFat];

            IList<DailyProgress> dailyProgressList = [];

            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                // Tìm kế hoạch hàng ngày tương ứng với ngày hiện tại
                var dailyPlan = dailyList.FirstOrDefault(d => d.Date == date);

                // Tạo CaloriesDTO cho ngày hiện tại
                var dailyProgress = new DailyProgress
                {
                    Date = date,
                    Calories = dailyPlan?.TotalCalories ?? 0f, // Nếu không có dữ liệu, Total = 0
                    Goal = dailyPlan?.TargetCalories ?? 0f // Nếu không có dữ liệu, Target = 0
                };

                dailyProgressList.Add(dailyProgress);
            }

            DateOnly startOfPeriod;
            DateOnly endOfPeriod;

            IList<Nutrition> nutritionList = [];

            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                startOfPeriod = today.AddDays(-7);
                endOfPeriod = today.AddDays(-1);
                var dailyListB = await _manager.DailyPlan.GetDailyPlanInWeekAsync(userId, startOfPeriod, endOfPeriod, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, startOfPeriod);

                var user = await _manager.DailyPlan.GetUserAsync(userId, true);
                var tdee = user.TDEE;

                float totalCalories = 0f;
                int validDays = 0;

                IList<WeightList> weightList = [];

                for (var date = startOfPeriod; date <= endOfPeriod; date = date.AddDays(1))
                {
                    var dailyPlan = dailyListB.FirstOrDefault(d => d.Date == date);
                    if (dailyPlan != null && Math.Abs(dailyPlan.TotalCalories - dailyPlan.TargetCalories) <= 500)
                    {
                        totalCalories += dailyPlan.TotalCalories;
                        validDays++;
                    }
                    WeightList weightL = new()
                    {
                        Date = date,
                        Value = 0f
                    };

                    Nutrition nutrition = new()
                    {
                        Date = date,
                        Carbs = dailyPlan.TotalCarbs,
                        Fat = dailyPlan.TotalFats,
                        Protein = dailyPlan.TotalProteins
                    };

                    nutritionList.Add(nutrition);

                    weightList.Add(weightL);
                }

                float calorieBalance = totalCalories - ((float)tdee * validDays);
                float weight = calorieBalance / 7700;

                float oldWeight = (float)user.Weight;

                if (user.TargetWeight > user.Weight)
                {
                    user.Weight += weight;
                }
                else
                {
                    user.Weight -= weight;
                }

                var result1 = new WeeklyProgressDTO
                {
                    Calories = caloriesDTO,
                    DailyProgresses = dailyProgressList,
                    Weight = new WeightDTO
                    {
                        Current = (float)user.Weight,
                        Change = Math.Abs((float)user.Weight - oldWeight),
                        Data = weightList
                    },
                    Macros = macros,
                    Nutrtion = nutritionList
                };

                await _manager.SaveAsync();

                return result1;
            }

            var result = new WeeklyProgressDTO
            {
                Calories = caloriesDTO,
            };

            return result;
        }
    }
}
