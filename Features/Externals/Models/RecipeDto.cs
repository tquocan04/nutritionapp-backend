namespace Features.Externals.Models;

public class RecipeDto
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public float Rating { get; set; }
    public string Directions { get; set; }
    public string NutritionRaw { get; set; }
    public List<string> Ingredients { get; set; } = [];
}