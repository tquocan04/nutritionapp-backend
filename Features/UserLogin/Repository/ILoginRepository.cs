using Domains;
using Features.UserLogin.Requests;

namespace Features.UserLogin.Repository
{
    public interface ILoginRepository
    {
        Task<User?> CheckLogin(LoginRequest login);
        Task<User?> Getuser(Guid? id);
    }
}
