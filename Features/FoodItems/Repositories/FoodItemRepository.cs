using Datas;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Features.FoodItems.Repositories
{
    public class FoodItemRepository(Context context) : IFoodItemRepository
    {
        private readonly Context _context = context;

        public async Task<DailyPlan?> GetDailyPlanAsync(Guid userId, DateOnly date, bool tracking)
        {
            if (!tracking)
            {
                return await _context.DailyPlans
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dp => dp.User_id == userId && dp.Date == date);
            }

            return await _context.DailyPlans
                .FirstOrDefaultAsync(dp => dp.User_id == userId && dp.Date == date);

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

        public async Task<IEnumerable<ItemBreakfast>?> GetBreakfastItemListAsync(Guid breakfastId, bool tracking)
        {
            if (!tracking)
            {
                return await _context.ItemBreakfasts
                    .AsNoTracking()
                    .Where(b => b.Breakfast_id == breakfastId)
                    .ToListAsync();
            }

            return await _context.ItemBreakfasts
                    .Where(b => b.Breakfast_id == breakfastId)
                    .ToListAsync();
        }
        
        public async Task<IEnumerable<ItemLunch>?> GetLunchItemListAsync(Guid lunchId, bool tracking)
        {
            if (!tracking)
            {
                return await _context.ItemLunches
                    .AsNoTracking()
                    .Where(l => l.Lunch_id == lunchId)
                    .ToListAsync();
            }

            return await _context.ItemLunches
                    .Where(l => l.Lunch_id == lunchId)
                    .ToListAsync();
        }
        
        public async Task<IEnumerable<ItemDinner>?> GetDinnerItemListAsync(Guid dinnerId, bool tracking)
        {
            if (!tracking)
            {
                return await _context.ItemDinners
                    .AsNoTracking()
                    .Where(d => d.Dinner_id == dinnerId)
                    .ToListAsync();
            }

            return await _context.ItemDinners
                    .Where(d => d.Dinner_id == dinnerId)
                    .ToListAsync();
        }
    }
}
