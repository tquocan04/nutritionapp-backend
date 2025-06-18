using Nest;
using System.Collections.Generic;

namespace Features.Externals.Models;

[ElasticsearchType(IdProperty = nameof(Id))]
public class FoodRecipeDocument
{
    public int Id { get; set; }

    [Text(Name = "name")]
    public string Name { get; set; } = null!;

    [Text(Name = "description")]
    public string Description { get; set; } = string.Empty!;

    [DenseVector(Dimensions = 1536, Name = "description_vector")]
    public float[]? DescriptionVector { get; set; }

    [Nested(Name = "ingredients")]
    public List<RecipeIngredient> Ingredients { get; set; } = [];
}

// Class này chỉ chứa thông tin sử dụng trong công thức
public class RecipeIngredient
{
    // Dùng Keyword để tham chiếu chính xác đến 'name' trong MasterIngredient
    [Keyword(Name = "name")]
    public string Name { get; set; } = null!;

    [Number(NumberType.Float, Name = "quantity_in_grams")]
    public float QuantityInGrams { get; set; }
}