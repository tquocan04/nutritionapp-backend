using Nest;

namespace Features.Externals.Models;

public class Ingredient
{
    [Text(Name = "raw_text")]
    public string RawText { get; set; } = null!;
}