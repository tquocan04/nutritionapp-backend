using Features.UserLogin.Requests;

namespace Features.UserLogin.Services
{
    public interface ILoginService
    {
        Task<(string, string)> Login(LoginRequest login);
    }
}
