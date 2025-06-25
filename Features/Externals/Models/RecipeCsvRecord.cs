using CsvHelper.Configuration.Attributes;

namespace Features.Externals.Models;

public class RecipeCsvRecord
{
    [Name("STT")]
    public string Stt { get; set; } = string.Empty;

    [Name("recipe_name")]
    public string RecipeName { get; set; } = string.Empty;

    [Name("prep_time")]
    public string PrepTime { get; set; } = string.Empty;

    [Name("cook_time")]
    public string CookTime { get; set; } = string.Empty;

    [Name("total_time")]
    public string TotalTime { get; set; } = string.Empty;

    [Name("servings")]
    public string Servings { get; set; } = string.Empty;

    [Name("yield")]
    public string Yield { get; set; } = string.Empty;

    [Name("ingredients")]
    public string Ingredients { get; set; } = string.Empty;

    [Name("directions")]
    public string Directions { get; set; } = null!;

    [Name("rating")]
    public string Rating { get; set; } = null!;

    [Name("url")]
    public string Url { get; set; } = null!;

    [Name("nutrition")]
    public string Nutrition { get; set; } = null!;

    [Name("img_src")]
    public string ImgSrc { get; set; } = null!;

    [Name("calories")]
    public string Calories { get; set; } = string.Empty;

    [Name("protein")]
    public string Protein { get; set; } = string.Empty;

    [Name("carbs")]
    public string Carbs { get; set; } = string.Empty;

    [Name("fat")]
    public string Fat { get; set; } = string.Empty;

    [Name("usda_ingredients_per_100g")]
    public string Usde_Ingredients_Per_100g { get; set; } = string.Empty;
}