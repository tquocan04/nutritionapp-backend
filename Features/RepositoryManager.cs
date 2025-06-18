using Datas;
using Features.DailyJobs.Repositories;
using Features.FoodItems.Repositories;
using Features.UserFeatures.Repository;
using Features.UserLogin.Repository;

namespace Features
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Context _context;
        private readonly Lazy<ILoginRepository> _loginRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IFoodItemRepository> _foodItemRepository;
        private readonly Lazy<IDailyPlanRepository> _dailyPlanRepository;

        public RepositoryManager(Context context)
        {
            _context = context;
            _loginRepository = new Lazy<ILoginRepository>(() => new LoginRepository(_context));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
            _foodItemRepository = new Lazy<IFoodItemRepository>(() => new FoodItemRepository(_context));
            _dailyPlanRepository = new Lazy<IDailyPlanRepository>(() => new DailyPlanRepository(_context));
        }
        public ILoginRepository Login => _loginRepository.Value;

        public IUserRepository User => _userRepository.Value;

        public IFoodItemRepository FoodItem => _foodItemRepository.Value;

        public IDailyPlanRepository DailyPlan => _dailyPlanRepository.Value;

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
