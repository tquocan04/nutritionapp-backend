namespace Features.Externals.Models;

public class RecipeDetailDto : RecipeDto
{
    public float Rating { get; set; }
    public string Directions { get; set; }
    public string NutritionRaw { get; set; }
    public List<string> Ingredients { get; set; } = [];
}

public class RecipeDto
{
    public string Id { get; set; } = string.Empty;
    public string Recipe_name { get; set; } = null!;
    public string Img_src { get; set; } = string.Empty;
}