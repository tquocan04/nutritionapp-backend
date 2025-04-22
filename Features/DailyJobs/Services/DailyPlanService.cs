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

            var caloriesList = new List<CaloriesDTO>();

            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                // Tìm kế hoạch hàng ngày tương ứng với ngày hiện tại
                var dailyPlan = dailyList.FirstOrDefault(d => d.Date == date);

                // Tạo CaloriesDTO cho ngày hiện tại
                var calories = new CaloriesDTO
                {
                    Date = date,
                    Total = dailyPlan?.TotalCalories ?? 0f, // Nếu không có dữ liệu, Total = 0
                    Target = dailyPlan?.TargetCalories ?? 0f // Nếu không có dữ liệu, Target = 0
                };

                caloriesList.Add(calories);
            }

            DateOnly startOfPeriod;
            DateOnly endOfPeriod;

            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                startOfPeriod = today.AddDays(-7);
                endOfPeriod = today.AddDays(-1);
                var dailyListB = await _manager.DailyPlan.GetDailyPlanInWeekAsync(userId, startOfPeriod, endOfPeriod, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, startOfPeriod);

                var user = await _manager.DailyPlan.GetUserAsync(userId, false);
                var tdee = user.TDEE;

                float totalCalories = 0f;
                int validDays = 0;

                for (var date = startOfPeriod; date <= endOfPeriod; date = date.AddDays(1))
                {
                    var dailyPlan = dailyListB.FirstOrDefault(d => d.Date == date);
                    if (dailyPlan != null && Math.Abs(dailyPlan.TotalCalories - dailyPlan.TargetCalories) <= 500)
                    {
                        totalCalories += dailyPlan.TotalCalories;
                        validDays++;
                    }
                }

                float calorieBalance = totalCalories - ((float)tdee * validDays);
                float weight = calorieBalance / 7700;

                var result1 = new WeeklyProgressDTO
                {
                    Calories = caloriesList,
                    Weight = new WeightDTO
                    {
                        Weight = weight
                    }
                };

                return result1;
            }

            var result = new WeeklyProgressDTO
            {
                Calories = caloriesList,
            };

            return result;
        }
    }
}
