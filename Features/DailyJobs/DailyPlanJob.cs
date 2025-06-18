using Datas;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Features.DailyJobs
{
    public class DailyPlanJob(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task Execute()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<Context>();

                // Lấy tất cả user
                var users = await context.Users.ToListAsync();
                if (users.Count == 0)
                {
                    return;
                }

                foreach (var user in users)
                {
                    // Kiểm tra xem user đã có DailyPlan cho ngày hôm nay chưa
                    var today = DateOnly.FromDateTime(DateTime.Today);
                    var existingPlan = await context.DailyPlans
                        .AsNoTracking()
                        .FirstOrDefaultAsync(dp => dp.User_id == user.Id && dp.Date == today);

                    if (existingPlan != null)
                    {
                        continue;
                    }

                    if (user.Time == 0 || user.TDEE == null || user.Weight == null)
                    {
                        continue;
                    }

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
                    var breakfastTask = context.Breakfasts.AddAsync(breakfast).AsTask();
                    var lunchTask = context.Lunches.AddAsync(lunch).AsTask();
                    var dinnerTask = context.Dinners.AddAsync(dinner).AsTask();
                    await Task.WhenAll(breakfastTask, lunchTask, dinnerTask);
                    
                    // DailyPlan
                    var dailyPlan = new DailyPlan
                    {
                        Id = dailyId,
                        User_id = user.Id,
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

                    context.DailyPlans.Add(dailyPlan);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
