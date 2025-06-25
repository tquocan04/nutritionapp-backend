using Elasticsearch.Net;
using Features.Externals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System.Text.Json;

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
                Id = doc.Stt,
                Img_src = doc.ImageUrl,
                Recipe_name = doc.RecipeName,
                //Directions = doc.Directions,
                //NutritionRaw = doc.NutritionRaw,
                //// Lấy danh sách chuỗi thành phần thô để hiển thị
                //Ingredients = doc.Ingredients.Select(i => i.RawText).ToList()
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

        public async Task<RecipeDetailDto?> GetRecommendationRecipeDetailAsync(string id)
        {
            var sttToSearch = id.ToString();

            // Dùng Search API với một câu lệnh Term Query để tìm chính xác
            var response = await _elasticClient.SearchAsync<RecipeDocument>(s => s
                .Index("recipes") // Tìm trong index recipes
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Stt) // Trên trường 'stt'
                        .Value(sttToSearch)      // Với giá trị bằng với ID được truyền vào
                    )
                )
                .Size(1) // Chỉ cần tìm 1 kết quả
            );

            // Lấy document đầu tiên được tìm thấy (nếu có)
            var source = response.Documents.FirstOrDefault();

            if (source == null)
            {
                _logger.LogWarning("Không tìm thấy công thức với STT: {RecipeId}", id);
                return null;
            }

            // tách direction thành 1 mảng
            string[] sentences = source.Directions.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);

            List<string> cleanedDirection = sentences
                                            .Select(step => step.Trim()) // Loại bỏ các khoảng trắng thừa ở đầu và cuối mỗi dòng.
                                            .Where(step => !string.IsNullOrWhiteSpace(step)) // Lọc bỏ các dòng hoàn toàn rỗng.
                                            .ToList();

            if (cleanedDirection.Count > 0)
            {
                // Xóa phần tử ở vị trí cuối cùng. 
                cleanedDirection.RemoveAt(cleanedDirection.Count - 1);
            }


            // TÁCH INGREDIENT_GRAMS THÀNH TỪNG CẶP GIÁ TRỊ NAME-GRAMS
            var resultList = MergeAndParse(source.IngredientGrams, source.Usde_Ingredients_Per_100g);

            // Ánh xạ (map) dữ liệu từ RecipeDocument (dữ liệu thô từ ES)
            // sang RecipeDetailDto (dữ liệu sạch để trả về cho client).
            var recipeDetail = new RecipeDetailDto
            {
                Id = source.Stt,
                Recipe_name = source.RecipeName,
                Prep_time = source.PrepTime,
                Cook_time = source.CookTime,
                Total_time = source.TotalTime,
                Servings = source.Servings,
                Yield = source.Yield,
                Directions = cleanedDirection,
                Timing = source.Timing,
                Nutrition = source.NutritionRaw,
                Calories = source.Calories,
                Protein = source.Protein,
                Carbs = source.Carbs,
                Fat = source.Fat,
                Img_src = source.ImageUrl,
                Ingredient_Grams = resultList
                // Chuyển đổi danh sách đối tượng thành danh sách chuỗi
                //Ingredients = source.Ingredients.Select(i => i.RawText).ToList(),
            };

            return recipeDetail;
        }

        /// <summary>
        /// Kết hợp và phân tích hai chuỗi dữ liệu thành phần để tạo ra một danh sách hoàn chỉnh.
        /// </summary>
        public static List<IngredientGrams> MergeAndParse(string gramsString, string nutritionString)
        {
            // 1. Phân tích chuỗi khối lượng thành một Dictionary để tra cứu nhanh
            var gramsLookup = ParseGramsString(gramsString);

            // 2. Phân tích chuỗi dinh dưỡng và kết hợp với dữ liệu khối lượng
            return ParseNutritionString(nutritionString, gramsLookup);
        }

        /// <summary>
        /// Helper để parse chuỗi "name: grams"
        /// </summary>
        private static Dictionary<string, string> ParseGramsString(string input)
        {
            var dictionary = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(input)) return dictionary;

            var pairs = input.Split(',');
            foreach (var pair in pairs)
            {
                var parts = pair.Split(':', 2);
                if (parts.Length == 2)
                {
                    var name = parts[0].Trim().ToLower(); // Chuẩn hóa về chữ thường để dễ so sánh
                    var grams = parts[1].Trim();
                    dictionary[name] = grams;
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Helper để parse chuỗi nutrition và tìm kiếm khối lượng tương ứng
        /// </summary>
        private static List<IngredientGrams> ParseNutritionString(string input, Dictionary<string, string> gramsLookup)
        {
            var resultList = new List<IngredientGrams>();
            if (string.IsNullOrWhiteSpace(input)) return resultList;

            var entries = input.Split(["}, "], StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split([": {"], 2, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                var name = parts[0].Trim();
                var jsonData = "{" + parts[1].Trim();
                if (!jsonData.EndsWith('}')) jsonData += "}";
                var validJsonData = jsonData.Replace("'", "\"");

                try
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var nutritionData = System.Text.Json.JsonSerializer.Deserialize<UsdaIngredientsPer100g>(validJsonData, options);

                    if (nutritionData != null)
                    {
                        // Tìm khối lượng tương ứng bằng cách đối chiếu thông minh
                        var grams = FindMatchingGrams(name, gramsLookup);

                        resultList.Add(new IngredientGrams
                        {
                            Name = name,
                            Grams = grams,
                            Usda_ingredients_per_100g = nutritionData
                        });
                    }
                }
                catch (System.Text.Json.JsonException) { /* Bỏ qua nếu JSON không hợp lệ */ }
            }
            return resultList;
        }

        /// <summary>
        /// Tìm kiếm thông minh: kiểm tra xem tên này có chứa hoặc được chứa bởi một key trong dictionary không
        /// </summary>
        private static string FindMatchingGrams(string nameToFind, Dictionary<string, string> gramsLookup)
        {
            var searchKey = nameToFind.ToLower();

            // Thử tìm kiếm khớp chính xác trước
            if (gramsLookup.TryGetValue(searchKey, out var exactMatch))
            {
                return exactMatch;
            }

            // Nếu không, thử tìm kiếm "chứa"
            foreach (var kvp in gramsLookup)
            {
                if (kvp.Key.Contains(searchKey) || searchKey.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }

            return "0g"; // Trả về giá trị mặc định nếu không tìm thấy
        }
    }
}
