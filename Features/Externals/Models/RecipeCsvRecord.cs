using CsvHelper.Configuration.Attributes;

namespace Features.Externals.Models;

public class RecipeCsvRecord
{
    [Name("recipe_name")]
    public string RecipeName { get; set; } = string.Empty;

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
}