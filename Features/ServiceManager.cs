using Features.UserLogin.Services;
using Microsoft.Extensions.Configuration;

namespace Features
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ILoginService> _loginService;

        public ServiceManager(IRepositoryManager repositoryManager, IConfiguration configuration) 
        {
            _loginService = new Lazy<ILoginService>(() => new LoginService(repositoryManager, configuration));
        }
        public ILoginService LoginService => _loginService.Value;
    }
}
