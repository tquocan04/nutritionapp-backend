using Domains;
using Features.UserFeatures.Requests.Register;

namespace Features.UserFeatures.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CheckEmailExist(RegisterRequest req);
        Task AddNewBreakfastAsync(Breakfast item);

        Task AddNewLunchAsync(Lunch item);

        Task AddNewDinnerAsync(Dinner item);
        Task AddNewDailyPlanAsync(DailyPlan req);
    }
}
