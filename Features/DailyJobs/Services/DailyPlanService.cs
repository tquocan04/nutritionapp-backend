using AutoMapper;
using Features.DailyJobs.DTOs;
using Features.DailyJobs.Exceptions;
using Features.DailyJobs.Requests;

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

            var daily = await _manager.DailyPlan.GetDailyPlanAsync(userId, today, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, startOfWeek);

            CaloriesDTO caloriesDTO = new()
            {
                Current = (float)Math.Floor((decimal)daily.TotalCalories),
                Target = (float)Math.Floor((decimal)daily.TargetCalories)
            };

            Macros macrosCarbs = new()
            {
                Name = "Carbs",
                Value = (float)Math.Floor((decimal)daily.TotalCarbs)
            };

            Macros macrosPro = new()
            {
                Name = "Protein",
                Value = (float)Math.Floor((decimal)daily.TotalProteins)
            };

            Macros macrosFat = new()
            {
                Name = "Fat",
                Value = (float)Math.Floor((decimal)daily.TotalFats)
            };

            IList<Macros> macros = [macrosCarbs, macrosPro, macrosFat];

            IList<DailyProgress> dailyProgressList = [];

            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                // Tìm kế hoạch hàng ngày tương ứng với ngày hiện tại
                var dailyPlan = dailyList.FirstOrDefault(d => d.Date == date);

                // Tạo CaloriesDTO cho ngày hiện tại
                //if (dailyPlan != null)
                //{
                    var dailyProgress = new DailyProgress
                    {
                        Date = date.ToString("dd/MM"),
                        Calories = dailyPlan?.TotalCalories ?? 0f, // Nếu không có dữ liệu, Total = 0
                        Goal = dailyPlan?.TargetCalories ?? 0f // Nếu không có dữ liệu, Target = 0
                    };

                    dailyProgressList.Add(dailyProgress);
            }

            DateOnly startOfPeriod;
            DateOnly endOfPeriod;

            IList<Nutrition> nutritionList = [];

            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                startOfPeriod = today.AddDays(-1);
                endOfPeriod = today.AddDays(+5);
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
                    if (date == today)
                    {
                        WeightList weightL = new()
                        {
                            Date = date.ToString("dd/MM"),
                            Value = (float)Math.Floor((decimal)user.Weight)
                        };
                        weightList.Add(weightL);
                    }
                    else
                    {
                        WeightList weightL = new()
                        {
                            Date = date.ToString("dd/MM"),
                            Value = 0f
                        };
                        weightList.Add(weightL);
                    }
                    
                    if (dailyPlan != null)
                    {
                        Nutrition nutrition = new()
                        {
                            Date = date.ToString("dd/MM"),
                            Carbs = (float)Math.Floor((decimal)dailyPlan.TotalCarbs),
                            Fat = (float)Math.Floor((decimal)dailyPlan.TotalFats),
                            Protein = (float)Math.Floor((decimal)dailyPlan.TotalProteins)
                        };
                        nutritionList.Add(nutrition);
                    }
                    else
                    {
                        Nutrition nutrition = new()
                        {
                            Date = date.ToString("dd/MM"),
                            Carbs = 0f,
                            Fat = 0f,
                            Protein = 0f
                        };
                        nutritionList.Add(nutrition);
                    }
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
                        Current = (float)Math.Floor((decimal)user.Weight),
                        Change = Math.Abs((float)user.Weight - oldWeight),
                        Data = weightList
                    },
                    Macros = macros,
                    Nutrtion = nutritionList
                };

                return result1;
            }

            var result = new WeeklyProgressDTO
            {
                Calories = caloriesDTO,
                DailyProgresses = dailyProgressList,
                Macros = macros,
            };

            return result;
        }
    }
}
