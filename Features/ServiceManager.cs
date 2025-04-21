using AutoMapper;
using Features.FoodItems.Services;
using Features.UserFeatures.Service;
using Features.UserLogin.Services;
using Microsoft.Extensions.Configuration;

namespace Features
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ILoginService> _loginService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IFoodItemService> _foodItemService;

        public ServiceManager(IRepositoryManager repositoryManager, IConfiguration configuration, IMapper mapper) 
        {
            _loginService = new Lazy<ILoginService>(() => new LoginService(repositoryManager, configuration, mapper));
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper));
            _foodItemService = new Lazy<IFoodItemService>(() => new FoodItemService(repositoryManager, mapper));
        }
        public ILoginService LoginService => _loginService.Value;

        public IUserService UserService => _userService.Value;

        public IFoodItemService FoodItemService => _foodItemService.Value;
    }
}
