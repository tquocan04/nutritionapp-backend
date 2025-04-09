using Domains;
using Features.UserLogin.Requests;

namespace Features.UserLogin.Services
{
    public interface ILoginService
    {
        Task<(string, User)> Login(LoginRequest login);
        Task<User> InformationDetail(InformationDetailRequest req, Guid id);
    }
}
