using RecipesAPI.Api.Dtos;
using RecipesAPI.Api.Models;

namespace RecipesAPI.Api.Data{
    
    public interface IAppRepo{
        Task CreateRecipeAsync(Recipe recipe);

        Task<Recipe?> GetRecipeByIdAsync(string id);

        Task<IEnumerable<Recipe?>?> GetAllRecipesAsync();

        Task<User?> CreateUserAsync(RegisterDto registerDto);

        Task<string?> GetUserKeyAsync(LoginDto loginDto);

        Task<bool> ValidKeyAsync(string key);

        Task<IEnumerable<User?>?> GetAllUsersAsync();
    } 
}