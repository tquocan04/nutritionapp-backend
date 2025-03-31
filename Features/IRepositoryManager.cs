using Features.UserLogin.Repository;

namespace Features
{
    public interface IRepositoryManager
    {
        ILoginRepository Login { get; }

        Task Save();
    }
}
