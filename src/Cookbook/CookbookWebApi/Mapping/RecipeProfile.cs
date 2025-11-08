using AutoMapper;
using CookbookDB.Models;

namespace CookbookWebApi.Mapping
{
    public class RecipeProfile: Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, CookbookDB.DTO.Recipe>()
                .ForMember(dest => dest.Ingredients,
                    opt => opt.MapFrom((src, dest, destMember, context) => MapIngredients(src)));
        }

        private static List<CookbookDB.DTO.Ingredient> MapIngredients(Recipe source)
        {
            return source.RecipeIngredients.Select(ri =>
            { 
                return new CookbookDB.DTO.Ingredient
                {
                    Id = ri.IngredientId,
                    Name = ri.Ingredient.Name,
                    Protein = ri.Ingredient.Protein,
                    Fats = ri.Ingredient.Fats,
                    Carbs = ri.Ingredient.Carbs,
                    Calories = ri.Ingredient.Calories,
                    Weight = ri.Weight
                };
            }).ToList();
        }
    }
}
