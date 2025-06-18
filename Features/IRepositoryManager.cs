using Features.DailyJobs.Repositories;
using Features.FoodItems.Repositories;
using Features.UserFeatures.Repository;
using Features.UserLogin.Repository;

namespace Features
{
    public interface IRepositoryManager
    {
        ILoginRepository Login { get; }
        IUserRepository User { get; }
        IFoodItemRepository FoodItem { get; }
        IDailyPlanRepository DailyPlan { get; }
        Task SaveAsync();
    }
}
