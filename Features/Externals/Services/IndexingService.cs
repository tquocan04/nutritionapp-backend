using CsvHelper;
using CsvHelper.Configuration;
using Features.Externals.Models;
using Microsoft.Extensions.Logging;
using Nest;
using System.Globalization;

namespace Features.Externals.Services;

public class IndexingService(IElasticClient elasticClient, IEmbeddingService embeddingService, ILogger<IndexingService> logger) : IIndexingService
{
    private readonly IElasticClient _elasticClient = elasticClient;
    private readonly IEmbeddingService _embeddingService = embeddingService;
    private readonly ILogger<IndexingService> _logger = logger;

    public async Task ProcessFileAsync(Stream fileStream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true, // File has title row
        };
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<RecipeCsvRecord>().ToList();
        var documentsToIndex = new List<RecipeDocument>();

        foreach (var record in records)
        {
            var ingredientPhrases = record.Ingredients.Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            if (ingredientPhrases.Count == 0) continue;

            var ingredientVectors = new List<float[]>();
            foreach (var phrase in ingredientPhrases)
            {
                var vector = await _embeddingService.GetEmbeddingAsync(phrase);
                if (vector != null) ingredientVectors.Add(vector);
            }

            if (ingredientVectors.Count == 0) continue;

            var averageVector = CalculateAverageVector(ingredientVectors);
            _ = float.TryParse(record.Rating, out float rating);
            _ = float.TryParse(record.Calories, out float calories);
            _ = float.TryParse(record.Carbs, out float carbs);
            _ = float.TryParse(record.Fat, out float fat);
            _ = float.TryParse(record.Protein, out float protein);
            _ = int.TryParse(record.Servings, out int servings);

            var doc = new RecipeDocument
            {
                Stt = record.Stt,
                RecipeName = record.RecipeName,
                Url = record.Url,
                ImageUrl = record.ImgSrc,
                CookTime = record.CookTime,
                PrepTime = record.PrepTime,
                TotalTime = record.TotalTime,
                Servings = servings,
                Yield = record.Yield,
                Rating = rating,
                Directions = record.Directions,
                NutritionRaw = record.Nutrition,
                Calories = calories,
                Carbs = carbs,
                Fat = fat,
                Protein = protein,
                Usde_Ingredients_Per_100g = record.Usde_Ingredients_Per_100g,
                Ingredients = ingredientPhrases.Select(p => new Ingredient { RawText = p }).ToList(),
                IngredientVector = averageVector
            };
            documentsToIndex.Add(doc);
        }

        if (documentsToIndex.Count != 0)
        {
            var bulkResponse = await _elasticClient.BulkAsync(b => b.Index("recipes").IndexMany(documentsToIndex));
            if (bulkResponse.IsValid)
            {
                _logger.LogInformation("Đã index thành công {Count} công thức.", documentsToIndex.Count);
            }
            else
            {
                _logger.LogError("Lỗi bulk indexing: {Error}", bulkResponse.DebugInformation);
            }
        }
    }

    private static float[]? CalculateAverageVector(List<float[]> vectors)
    {
        if (vectors.Count == 0) return null;
        int dimensions = vectors.First().Length;
        var sumVector = new float[dimensions];
        foreach (var vector in vectors.Where(v => v.Length == dimensions))
        {
            for (int j = 0; j < dimensions; j++) sumVector[j] += vector[j];
        }
        for (int j = 0; j < dimensions; j++) sumVector[j] /= vectors.Count;
        return sumVector;
    }
}