namespace Features.Externals.Services
{
    public interface IIndexingService
    {
        Task ProcessFileAsync(Stream fileStream);
    }
}
