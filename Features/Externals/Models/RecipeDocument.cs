using Nest;

namespace Features.Externals.Models;

[ElasticsearchType(IdProperty = nameof(Url))]
public class RecipeDocument
{
    [Text(Name = "title")]
    public string Title { get; set; } = null!;

    [Keyword(Name = "url")]
    public string Url { get; set; } = null!;

    [Keyword(Name = "image_url")]
    public string ImageUrl { get; set; } = null!;

    [Number(NumberType.Float, Name = "rating")]
    public float Rating { get; set; }

    [Text(Name = "directions")]
    public string Directions { get; set; } = null!;

    [Text(Name = "nutrition_raw")]
    public string NutritionRaw { get; set; } = null!;

    [Nested(Name = "ingredients")]
    public List<Ingredient> Ingredients { get; set; } = [];

    [DenseVector(Dimensions = 768, Name = "ingredient_vector")]
    public float[]? IngredientVector { get; set; }
}