using System.Text.Json;

namespace Features.Externals.Models;

public class RecipeDetailDto : RecipeDto
{
    public string Prep_time { get; set; } = string.Empty;

    public string Cook_time { get; set; } = string.Empty;

    public string Total_time { get; set; } = string.Empty;
    public string Timing { get; set; } = string.Empty;

    public int Servings { get; set; }

    public string Yield { get; set; } = string.Empty;

    public List<string> Directions { get; set; } = [];

    public float Calories { get; set; } = 0f;

    public float Protein { get; set; } = 0f;

    public float Carbs { get; set; } = 0f;

    public float Fat { get; set; } = 0f;

    public string Nutrition { get; set; } = null!;
    public List<IngredientGrams> Ingredient_Grams { get; set; } = [];
}

public class RecipeDto
{
    public string Id { get; set; } = string.Empty;
    public string Recipe_name { get; set; } = null!;
    public string Img_src { get; set; } = string.Empty;
}

public class IngredientGrams
{
    public string Name { get; set; } = string.Empty;
    public string Grams { get; set; } = string.Empty;
    public UsdaIngredientsPer100g? Usda_ingredients_per_100g { get; set; }

    public static List<IngredientGrams> ParseFromString(string nutritionString)
    {
        var resultList = new List<IngredientGrams>();
        if (string.IsNullOrWhiteSpace(nutritionString))
        {
            return resultList;
        }

        // Tách chuỗi lớn thành các phần riêng lẻ cho mỗi thành phần.
        string[] ingredientEntries = nutritionString.Split(["}, "], StringSplitOptions.RemoveEmptyEntries);

        foreach (var entry in ingredientEntries)
        {
            // Tách mỗi phần thành tên thành phần và chuỗi JSON dữ liệu
            var parts = entry.Split([": {"], 2, StringSplitOptions.None);
            if (parts.Length != 2) continue;

            var name = parts[0].Trim();
            var jsonData = "{" + parts[1].Trim();

            // Đảm bảo chuỗi JSON được đóng đúng cách
            if (!jsonData.EndsWith("}"))
            {
                jsonData += "}";
            }

            // Chuyển chuỗi JSON không chuẩn (dùng nháy đơn) thành JSON chuẩn
            var validJsonData = jsonData.Replace("'", "\"");

            try
            {
                // Deserialize chuỗi JSON thành đối tượng UsdaIngredientsPer100g
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var nutritionData = JsonSerializer.Deserialize<UsdaIngredientsPer100g>(validJsonData, options);

                if (nutritionData != null)
                {
                    resultList.Add(new IngredientGrams
                    {
                        Name = name,
                        Grams = "100", // Tạm gán là "100" vì dữ liệu là "per 100g"
                        Usda_ingredients_per_100g = nutritionData
                    });
                }
            }
            catch (JsonException ex)
            {
                // Ghi log lỗi nếu cần
                Console.WriteLine($"Lỗi khi parse JSON cho '{name}': {ex.Message}");
            }
        }

        return resultList;
    }
}

public class UsdaIngredientsPer100g
{
    public float Calories { get; set; } = 0f;
    public float Protein { get; set; } = 0f;
    public float Carbs { get; set; } = 0f;
    public float Fat { get; set; } = 0f;
}