namespace CookbookDB.Models;

/// <summary>
/// рецепты в списках
/// </summary>
public partial class ListRecipe
{
    public int ListId { get; set; }

    public int RecipeId { get; set; }
}
