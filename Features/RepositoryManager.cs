using Datas;
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

        public RepositoryManager(Context context)
        {
            _context = context;
            _loginRepository = new Lazy<ILoginRepository>(() => new LoginRepository(_context));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
            _foodItemRepository = new Lazy<IFoodItemRepository>(() => new FoodItemRepository(_context));
        }
        public ILoginRepository Login => _loginRepository.Value;

        public IUserRepository User => _userRepository.Value;

        public IFoodItemRepository FoodItem => _foodItemRepository.Value;

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
