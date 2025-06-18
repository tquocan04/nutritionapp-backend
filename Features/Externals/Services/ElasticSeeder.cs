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
            _logger.LogInformation("Bắt đầu quá trình ElasticSeeder...");

            // Gọi hàm tạo index cho công thức
            await CreateFoodRecipesIndexAsync();

            // Gọi hàm tạo index cho từ điển thành phần
            await CreateIngredientsMasterIndexAsync();

            _logger.LogInformation("Quá trình ElasticSeeder hoàn tất.");
        }

        /// <summary>
        /// Tạo index 'food_recipes' nếu nó chưa tồn tại.
        /// </summary>
        private async Task CreateFoodRecipesIndexAsync()
        {
            const string indexName = "food_recipes";
            var indexExistsResponse = await _elasticClient.Indices.ExistsAsync(indexName);

            if (indexExistsResponse.Exists)
            {
                _logger.LogInformation("Index '{indexName}' đã tồn tại. Bỏ qua việc tạo mới.", indexName);
                return;
            }

            _logger.LogInformation("Index '{indexName}' chưa tồn tại. Đang tạo mới...", indexName);

            var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                // Sử dụng model FoodRecipeDocument để tự động tạo mapping
                .Map<FoodRecipeDocument>(m => m.AutoMap())
            );

            if (createIndexResponse.IsValid)
            {
                _logger.LogInformation("Tạo index '{indexName}' thành công.", indexName);
            }
            else
            {
                _logger.LogError("Tạo index '{indexName}' thất bại. Lý do: {debugInfo}", indexName, createIndexResponse.DebugInformation);
            }
        }

        /// <summary>
        /// Tạo index 'ingredients_master' nếu nó chưa tồn tại.
        /// </summary>
        private async Task CreateIngredientsMasterIndexAsync()
        {
            const string indexName = "ingredients_master";
            var indexExistsResponse = await _elasticClient.Indices.ExistsAsync(indexName);

            if (indexExistsResponse.Exists)
            {
                _logger.LogInformation("Index '{indexName}' đã tồn tại. Bỏ qua việc tạo mới.", indexName);
                return;
            }

            _logger.LogInformation("Index '{indexName}' chưa tồn tại. Đang tạo mới...", indexName);

            var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                // Sử dụng model MasterIngredient để tự động tạo mapping
                .Map<MasterIngredient>(m => m.AutoMap())
            );

            if (createIndexResponse.IsValid)
            {
                _logger.LogInformation("Tạo index '{indexName}' thành công.", indexName);
            }
            else
            {
                _logger.LogError("Tạo index '{indexName}' thất bại. Lý do: {debugInfo}", indexName, createIndexResponse.DebugInformation);
            }
        }
    }
}
