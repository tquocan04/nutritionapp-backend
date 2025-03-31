using Datas;
using Features.UserLogin.Repository;

namespace Features
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Context _context;
        private readonly Lazy<ILoginRepository> _loginRepository;
        public RepositoryManager(Context context)
        {
            _context = context;
            _loginRepository = new Lazy<ILoginRepository>(() => new LoginRepository(_context));
        }
        public ILoginRepository Login => _loginRepository.Value;
        public async Task Save() => await _context.SaveChangesAsync();
    }
}
