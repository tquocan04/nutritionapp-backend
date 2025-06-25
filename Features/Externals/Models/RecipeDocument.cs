using CsvHelper.Configuration.Attributes;
using Nest;

namespace Features.Externals.Models;

[ElasticsearchType(IdProperty = nameof(Url))]
public class RecipeDocument
{
    [Text(Name = "stt")]
    public string Stt { get; set; } = null!;
    
    [Text(Name = "recipe_name")]
    public string RecipeName { get; set; } = null!;

    [Text(Name = "prep_time")]
    public string PrepTime { get; set; } = string.Empty;

    [Text(Name = "cook_time")]
    public string CookTime { get; set; } = string.Empty;

    [Text(Name = "total_time")]
    public string TotalTime { get; set; } = string.Empty;

    [Number(NumberType.Integer, Name = "servings")]
    public int Servings { get; set; }

    [Text(Name = "yield")]
    public string Yield { get; set; } = string.Empty;

    [Keyword(Name = "url")]
    public string Url { get; set; } = null!;

    [Keyword(Name = "image_url")]
    public string ImageUrl { get; set; } = null!;

    [Number(NumberType.Float, Name = "rating")]
    public float Rating { get; set; }

    [Text(Name = "directions")]
    public string Directions { get; set; } = null!;

    [Number(NumberType.Float, Name = "calories")]
    public float Calories { get; set; }

    [Number(NumberType.Float, Name = "protein")]
    public float Protein { get; set; }

    [Number(NumberType.Float, Name = "carbs")]
    public float Carbs { get; set; }

    [Number(NumberType.Float, Name = "fat")]
    public float Fat { get; set; }

    [Text(Name = "usda_ingredients_per_100g")]
    public string Usde_Ingredients_Per_100g { get; set; } = string.Empty;

    [Text(Name = "nutrition_raw")]
    public string NutritionRaw { get; set; } = null!;

    [Nested(Name = "ingredients")]
    public List<Ingredient> Ingredients { get; set; } = [];

    [DenseVector(Dimensions = 768, Name = "ingredient_vector")]
    public float[]? IngredientVector { get; set; }
}