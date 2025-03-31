using Features.UserLogin.Services;

namespace Features
{
    public interface IServiceManager
    {
        ILoginService LoginService { get; }
    }
}
