using CookbookDB.Models;
using Microsoft.EntityFrameworkCore;
using Recipe = CookbookDB.Models.Recipe;

namespace CookbookDB.Repositories;
public class RecipeRepository(CookbookDbContext context)
{
    private readonly CookbookDbContext _context = context;

    public async Task<Recipe?> Get(int id)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recipe?> Get(string name)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<int> AddRecipeAsync(DTO.Recipe recipeDto)
    {
        var recipe = new Recipe
        {
            Name = recipeDto.Name,
            Instruction = recipeDto.Instruction,
            ServingsNumber = recipeDto.ServingsNumber
        };

        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();

        foreach (var ingredientDto in recipeDto.Ingredients)
        {
            Models.Ingredient? existingIngredient;
            RecipeIngredient recipeIngredient;

            //если ингредиент уже есть и передали id
            if (ingredientDto.Id.HasValue)
            {
                existingIngredient = await _context.Ingredients
               .FirstOrDefaultAsync(i => i.Id == ingredientDto.Id);
                if (existingIngredient != null)
                {
                    recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recipe.Id,
                        IngredientId = existingIngredient!.Id,
                        Weight = ingredientDto.Weight
                    };

                    await _context.RecipeIngredients.AddAsync(recipeIngredient);
                    continue;
                }
            }

            //если id не передали или по нему не нашли (рецепт из themeal), пробуем найти по имени
            existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name == ingredientDto.Name);

            Models.Ingredient ingredient;

            if (existingIngredient != null)
            {
                ingredient = existingIngredient;
            }
            else
            {
                //по имени тоже не нашли - создаем новый, потом придется заполнить информацию
                //todo: в dto хранить список id ингрдиентов, а не сущностей
                ingredient = new Models.Ingredient
                {
                    Name = ingredientDto.Name
                };

                await _context.Ingredients.AddAsync(ingredient);
                await _context.SaveChangesAsync();
            }

            recipeIngredient = new RecipeIngredient
            {
                RecipeId = recipe.Id,
                IngredientId = ingredient.Id,
                Weight = ingredientDto.Weight
            };

            await _context.RecipeIngredients.AddAsync(recipeIngredient);
        }

        await _context.SaveChangesAsync();

        return recipe.Id;
    }

    //public async Task InsertRecipeAsync(DTO.Recipe recipe)
    //{
    //    // Ensure the recipe and its ingredients get added to the database
    //    //var existingRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Name == recipe.Name);
    //    //if (existingRecipe == null)
    //    //{
    //    Recipe recipeModel = new()
    //    {
    //        Id = recipe.Id,
    //        Name = recipe.Name
    //    };
    //    var insertedRecipe = await _context.Recipes.AddAsync(recipeModel);
    //    var existingRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Name == recipe.Name);
    //    //await _context.SaveChangesAsync(); // Save the recipe first to generate an ID for the recipe
    //    foreach (var ingredient in recipe.Ingredients)
    //    {
    //        // Check if the ingredient already exists
    //        var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredient.Name);
    //        if (existingIngredient == null)
    //        {
    //            // Assuming you want to store additional nutritional information
    //            existingIngredient = new Models.Ingredient
    //            {
    //                Name = ingredient.Name,
    //            };
    //            await _context.Ingredients.AddAsync(existingIngredient);
    //            //await _context.SaveChangesAsync(); // Save the ingredient to generate an ID
    //            //existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredient.Name);
    //        }
    //        recipeModel.RecipeIngredients.Add(new RecipeIngredient
    //        {
    //            //RecipeId = insertedRecipe.Entity.Id,
    //            //IngredientId = existingIngredient!.Id, // Use existing or newly added ingredient
    //            Ingredient = existingIngredient,
    //            Weight = ingredient.Weight
    //        });


    //        //await _context.RecipeIngredients.AddAsync(recipeIngredient);
    //        //}

    //        await _context.SaveChangesAsync(); // Save all the recipe ingredients
    //    }
    //}
}