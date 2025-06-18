using Domains;
using Features.UserLogin.Requests;

namespace Features.UserLogin.Repository
{
    public interface ILoginRepository
    {
        Task<User?> CheckLogin(LoginRequest login);
        Task<User?> Getuser(Guid? id);
        Task<DailyPlan?> GetDailyPlanByUserId(Guid userId, DateOnly date, bool tracking);
        Task AddNewBreakfastAsync(Breakfast breakfast);
        Task AddNewLunchAsync(Lunch Lunch);
        Task AddNewDinnerAsync(Dinner Dinner);
        Task AddNewDailyPlanAsync(DailyPlan daily);
    }
}
