using Datas;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Features.FoodItems.Repositories
{
    public class FoodItemRepository(Context context) : IFoodItemRepository
    {
        private readonly Context _context = context;

        public async Task<DailyPlan?> GetDailyPlanAsync(DateOnly date, bool tracking)
        {
            if (!tracking)
            {
                return await _context.DailyPlans
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dp => dp.Date == date);
            }

            return await _context.DailyPlans
                .FirstOrDefaultAsync(dp => dp.Date == date);

        }

        public async Task<Breakfast?> GetBreakfastAsync(Guid id, bool tracking)
        {
            if (tracking)
            {
                return await _context.Breakfasts.FirstOrDefaultAsync(b => b.Id == id);
            }

            return await _context.Breakfasts
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Lunch?> GetLunchAsync(Guid id, bool tracking)
        {
            if (tracking)
            {
                return await _context.Lunches.FirstOrDefaultAsync(b => b.Id == id);
            }

            return await _context.Lunches
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Dinner?> GetDinnerAsync(Guid id, bool tracking)
        {
            if (tracking)
            {
                return await _context.Dinners.FirstOrDefaultAsync(b => b.Id == id);
            }

            return await _context.Dinners
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddNewBreakfastItemAsync(ItemBreakfast item)
        {
            await _context.ItemBreakfasts.AddAsync(item);
        }

        public async Task AddNewLunchItemAsync(ItemLunch item)
        {
            await _context.ItemLunches.AddAsync(item);
        }

        public async Task AddNewDinnerItemAsync(ItemDinner item)
        {
            await _context.ItemDinners.AddAsync(item);
        }
    }
}
