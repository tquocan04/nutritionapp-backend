using Features.UserFeatures.Service;
using Features.UserLogin.Services;

namespace Features
{
    public interface IServiceManager
    {
        ILoginService LoginService { get; }
        IUserService UserService { get; }
    }
}
