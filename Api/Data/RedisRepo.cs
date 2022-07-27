using System.Text.Json;
using RecipesAPI.Api.Models;
using StackExchange.Redis;

namespace RecipesAPI.Api.Data{

    public class RedisRepo : IAppRepo{
        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase database;

        public RedisRepo(IConnectionMultiplexer redis){
            _redis = redis; 
            database = _redis.GetDatabase();   
        }

        public async Task CreateRecipeAsync(Recipe recipe){
            var serialRecipe = JsonSerializer.Serialize(recipe);
            
            await database.HashSetAsync("Recipes", new HashEntry[]{
                new HashEntry(recipe.Id, serialRecipe)});
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync(){
            var allRecipes = await database.HashGetAllAsync("Recipes");

            if(allRecipes.Length == 0){
                return null;
            }

            return Array.ConvertAll(allRecipes, val => 
                JsonSerializer.Deserialize<Recipe>(val.Value)).ToList();
        }

        public async Task<Recipe> GetRecipeByIdAsync(string id){
            var recipe = await database.HashGetAsync("Recipes", id);

            if(string.IsNullOrEmpty(recipe)){
                return null;
            }

            return JsonSerializer.Deserialize<Recipe>(recipe);
        }
    }
}