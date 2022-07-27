using RecipesAPI.Api.Models;

namespace RecipesAPI.Api.Data{
    
    public interface IAppRepo{
        Task CreateRecipeAsync(Recipe recipe);

        Task<Recipe> GetRecipeByIdAsync(string id);

        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
    } 
}