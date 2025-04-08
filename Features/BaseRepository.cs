using Datas;

namespace Features
{
    public class BaseRepository<T>(Context context) : IBaseRepository<T> where T : class
    {
        private readonly Context _context = context;

        public async Task Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
