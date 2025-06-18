using Features.DailyJobs.Services;
using Features.FoodItems.Services;
using Features.UserFeatures.Service;
using Features.UserLogin.Services;

namespace Features
{
    public interface IServiceManager
    {
        ILoginService LoginService { get; }
        IUserService UserService { get; }
        IFoodItemService FoodItemService { get; }
        IDailyPlanService DailyPlanService { get; }
    }
}
