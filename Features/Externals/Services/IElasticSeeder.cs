namespace Features.Externals.Services
{
    public interface IElasticSeeder
    {
        /// <summary>
        /// Khởi chạy quá trình kiểm tra và tạo index cho Elasticsearch.
        /// </summary>
        Task SeedAsync();
    }
}
