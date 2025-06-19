using Features.Externals.Models;
using Microsoft.Extensions.Logging;
using Nest;

namespace Features.Externals.Services
{
    public class ElasticSeeder(IElasticClient elasticClient, ILogger<ElasticSeeder> logger) : IElasticSeeder
    {
        private readonly IElasticClient _elasticClient = elasticClient;
        private readonly ILogger<ElasticSeeder> _logger = logger;

        /// <summary>
        /// Phương thức công khai được gọi từ Program.cs
        /// Sẽ khởi chạy việc tạo tất cả các index cần thiết.
        /// </summary>
        public async Task SeedAsync()
        {
            _logger.LogInformation("Start the ElasticSeeder process...");

            await CreateRecipesIndexAsync();

            _logger.LogInformation("ElasticSeeder process completed.");
        }

        /// <summary>
        /// Tạo index 'recipes' nếu nó chưa tồn tại.
        /// </summary>
        private async Task CreateRecipesIndexAsync()
        {
            const string indexName = "recipes";
            var indexExistsResponse = await _elasticClient.Indices.ExistsAsync(indexName);

            if (indexExistsResponse.Exists)
            {
                _logger.LogInformation("Index '{indexName}' already exists.", indexName);
                return;
            }

            _logger.LogInformation("Index '{indexName}' does not exist. Creating...", indexName);

            var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                .Map<RecipeDocument>(m => m.AutoMap()) // Dùng model RecipeDocument
            );

            if (createIndexResponse.IsValid)
                _logger.LogInformation("Index '{indexName}' created successfully.", indexName);
            else
                _logger.LogError("Index '{indexName}' creation failed: {debugInfo}", indexName, createIndexResponse.DebugInformation);
        }
    }
}
