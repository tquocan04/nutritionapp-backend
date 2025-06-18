using Nest;

namespace Features.Externals.Models;

[ElasticsearchType(IdProperty = nameof(Name))]
public class MasterIngredient
{
    [Keyword(Name = "name")]
    public string Name { get; set; } = null!;

    [DenseVector(Dimensions = 1536, Name = "name_vector")]
    public float[]? NameVector { get; set; }

    [Object(Name = "nutrients_per_100g")]
    public NutrientInfo? NutrientsPer100g { get; set; }
}

public class NutrientInfo
{
    [Number(NumberType.Float, Name = "calories")]
    public float? Calories { get; set; }

    [Number(NumberType.Float, Name = "protein")]
    public float? Protein { get; set; }

    [Number(NumberType.Float, Name = "fat")]
    public float? Fat { get; set; }

    [Number(NumberType.Float, Name = "carbs")]
    public float? Carbs { get; set; }
}