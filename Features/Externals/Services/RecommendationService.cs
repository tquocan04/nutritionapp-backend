using Elasticsearch.Net;
using Features.Externals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;

namespace Features.Externals.Services
{
    public class RecommendationService(IElasticClient elasticClient, Datas.Context dbContext,
        IEmbeddingService embeddingService,
        ILogger<RecommendationService> logger) : IRecommendationService
    {
        private readonly IElasticClient _elasticClient = elasticClient;
        private readonly Datas.Context _dbContext = dbContext;
        private readonly IEmbeddingService _embeddingService = embeddingService;
        private readonly ILogger<RecommendationService> _logger = logger;

        public async Task<IEnumerable<RecipeDto>> GetRecommendationsForMealAsync(Guid userId)
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var currentTimeInVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            string mealType;
            if (currentTimeInVietnam.Hour >= 0 && currentTimeInVietnam.Hour < 9)
            {
                mealType = "breakfast";
            }
            else if (currentTimeInVietnam.Hour >= 9 && currentTimeInVietnam.Hour < 14)
            {
                mealType = "lunch";
            }
            else
            {
                mealType = "dinner";
            }
            // === BƯỚC A: LẤY TÊN THÀNH PHẦN TỪ SQL SERVER (Không đổi) ===
            List<string> ingredientNamesFromSql = [];
            Console.WriteLine("BƯỚC A");

            try
            {
                int numberOfRecentMeals = 5;
                var recentPlanIds = await _dbContext.DailyPlans
                    .AsNoTracking()
                    .Where(dp => dp.User_id == userId)
                    .OrderByDescending(dp => dp.Date)
                    .Take(numberOfRecentMeals)
                    .Select(dp => dp.Id)
                    .ToListAsync();

                if (recentPlanIds.Count == 0) return [];

                switch (mealType)
                {
                    case "breakfast":
                        var breakfastIds = await _dbContext.DailyPlans
                            .Where(dp => recentPlanIds.Contains(dp.Id))
                            .Select(dp => dp.Breakfast_id)
                            .ToListAsync();
                        ingredientNamesFromSql = await _dbContext.ItemBreakfasts
                            .Where(item => breakfastIds.Contains(item.Breakfast_id))
                            .Select(item => item.Name)
                            .Distinct()
                            .ToListAsync();
                        break;
                    case "lunch":
                        var lunchIds = await _dbContext.DailyPlans
                            .Where(dp => recentPlanIds.Contains(dp.Id))
                            .Select(dp => dp.Lunch_id)
                            .ToListAsync();
                        ingredientNamesFromSql = await _dbContext.ItemLunches
                            .Where(item => lunchIds.Contains(item.Lunch_id))
                            .Select(item => item.Name)
                            .Distinct()
                            .ToListAsync();
                        break;
                    case "dinner":
                        var dinnerIds = await _dbContext.DailyPlans
                            .Where(dp => recentPlanIds.Contains(dp.Id))
                            .Select(dp => dp.Dinner_id)
                            .ToListAsync();
                        ingredientNamesFromSql = await _dbContext.ItemDinners
                            .Where(item => dinnerIds.Contains(item.Dinner_id))
                            .Select(item => item.Name)
                            .Distinct()
                            .ToListAsync();
                        break;
                    default:
                        return [];
                }
            }
            finally
            {
                // DbContext sẽ tự động được giải phóng ở đây
            }
            if (ingredientNamesFromSql.Count == 0)
            {
                _logger.LogInformation("Không tìm thấy thành phần nào cho bữa ăn");
                return [];
            }

            // === BƯỚC B: TẠO VECTOR CHO TỪNG THÀNH PHẦN (Logic mới) ===
            _logger.LogInformation("Đang tạo vector cho {Count} thành phần từ bữa ăn...", ingredientNamesFromSql.Count);
            var mealIngredientVectors = new List<float[]>();
            foreach (var name in ingredientNamesFromSql.Distinct()) // Dùng Distinct để không phải gọi API cho các thành phần trùng lặp
            {
                var vector = await _embeddingService.GetEmbeddingAsync(name);
                if (vector != null)
                {
                    mealIngredientVectors.Add(vector);
                }
            }

            if (mealIngredientVectors.Count == 0)
            {
                _logger.LogWarning("Không thể tạo được vector cho bất kỳ thành phần nào trong bữa ăn.");
                return [];
            }

            // === BƯỚC C: TÍNH VECTOR TRUNG BÌNH (Không đổi) ===
            var averageVector = CalculateAverageVector(mealIngredientVectors);
            if (averageVector == null) return [];

            // === BƯỚC D: TÌM KIẾM BẰNG CÁCH GỬI JSON THÔ (Đã đổi lại) ===

            // 1. Tạo một đối tượng C# có cấu trúc JSON của câu lệnh k-NN
            var knnQueryObject = new
            {
                knn = new
                {
                    field = "ingredient_vector",
                    query_vector = averageVector,
                    k = 10, // Lấy 10 gợi ý
                    num_candidates = 100
                },
                _source = new
                {
                    excludes = new[] { "ingredient_vector" } // Không cần trả về trường vector
                }
            };

            // 2. Chuyển đối tượng trên thành chuỗi JSON
            var jsonQuery = JsonConvert.SerializeObject(knnQueryObject);

            // 3. Dùng LowLevel client để gửi request với chuỗi JSON đã tạo
            var searchResponse = await _elasticClient.LowLevel
                .SearchAsync<SearchResponse<RecipeDocument>>("recipes", PostData.String(jsonQuery));
            
            if (!searchResponse.IsValid)
            {
                _logger.LogError("Lỗi khi thực hiện k-NN search: {Error}", searchResponse.DebugInformation);
                return [];
            }

            var recommendations = searchResponse.Documents.Select(doc => new RecipeDto
            {
                Title = doc.Title,
                ImageUrl = doc.ImageUrl,
                Rating = doc.Rating,
                Directions = doc.Directions,
                NutritionRaw = doc.NutritionRaw,
                // Lấy danh sách chuỗi thành phần thô để hiển thị
                Ingredients = doc.Ingredients.Select(i => i.RawText).ToList()
            }).ToList();

            return recommendations;
        }

        private static float[]? CalculateAverageVector(List<float[]> vectors)
        {
            if (vectors == null || vectors.Count == 0) return null;
            int dimensions = vectors.First().Length;
            var sumVector = new float[dimensions];
            foreach (var vector in vectors)
            {
                if (vector.Length != dimensions) continue;
                for (int i = 0; i < dimensions; i++) { sumVector[i] += vector[i]; }
            }
            for (int i = 0; i < dimensions; i++) { sumVector[i] /= vectors.Count; }
            return sumVector;
        }
    }
}
