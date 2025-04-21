using Domains;

namespace Features.FoodItems.Repositories
{
    public interface IFoodItemRepository
    {
        Task<DailyPlan?> GetDailyPlanAsync(DateOnly date, bool tracking);
        Task<Breakfast?> GetBreakfastAsync(Guid id, bool tracking);
        Task AddNewBreakfastItemAsync(ItemBreakfast item);
        Task<Lunch?> GetLunchAsync(Guid id, bool tracking);
        Task AddNewLunchItemAsync(ItemLunch item);
        Task<Dinner?> GetDinnerAsync(Guid id, bool tracking);
        Task AddNewDinnerItemAsync(ItemDinner item);
    }
}
