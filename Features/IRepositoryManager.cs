using Features.UserFeatures.Repository;
using Features.UserLogin.Repository;

namespace Features
{
    public interface IRepositoryManager
    {
        ILoginRepository Login { get; }
        IUserRepository User { get; }
        Task SaveAsync();
    }
}
