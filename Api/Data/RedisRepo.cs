using System.Security.Cryptography;
using System.Text.Json;
using AutoMapper;
using RecipesAPI.Api.Dtos;
using RecipesAPI.Api.Models;
using StackExchange.Redis;

namespace RecipesAPI.Api.Data{

    public class RedisRepo : IAppRepo{
        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase database;

        private readonly IMapper _mapper;

        public RedisRepo(IConnectionMultiplexer redis, IMapper mapper){
            _redis = redis; 
            database = _redis.GetDatabase();
            _mapper = mapper;
        }

        public async Task CreateRecipeAsync(Recipe recipe){
            var serialRecipe = JsonSerializer.Serialize(recipe);
            
            await database.HashSetAsync("Recipes", new HashEntry[]{
                new HashEntry(recipe.Id, serialRecipe)});
        }

        public async Task<IEnumerable<Recipe?>?> GetAllRecipesAsync(){
            var allRecipes = await database.HashGetAllAsync("Recipes");

            if(allRecipes.Length == 0){
                return null;
            }

            return Array.ConvertAll(allRecipes, val => 
                JsonSerializer.Deserialize<Recipe>(val.Value)).ToList();
        }

        public async Task<Recipe?> GetRecipeByIdAsync(string id){
            var recipe = await database.HashGetAsync("Recipes", id);

            if(string.IsNullOrEmpty(recipe)){
                return null;
            }

            return JsonSerializer.Deserialize<Recipe>(recipe);
        }

        public async Task<User?> CreateUserAsync(RegisterDto registerDto){
            var allUsers = await GetAllUsersAsync();
            var userExist = allUsers.FirstOrDefault(x => x.EmailAddress == registerDto.EmailAddress);
            
            if(userExist != null){
                return null;
            }

            var user = _mapper.Map<User>(registerDto);
            
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            generator.GetBytes(key);
            string apiKey = Convert.ToBase64String(key);
            
            user.ApiKey = apiKey;

            var serialUser = JsonSerializer.Serialize(user);
            
            await database.HashSetAsync("Users", new HashEntry[]{
                new HashEntry(user.EmailAddress, serialUser)});

            return user;
        }

        public async Task<string?> GetUserKeyAsync(LoginDto loginDto){
            var users = await GetAllUsersAsync();

            if(users == null){
                return null;
            }

            var user = users.FirstOrDefault(x => x.EmailAddress == loginDto.EmailAddress && 
                                    x.Password == loginDto.Password);
        
            if(user == null){
                return null;
            }

            return user.ApiKey;
        }

        public async Task<bool> ValidKeyAsync(string key){
            var allUsers = await GetAllUsersAsync();

            var user = allUsers.FirstOrDefault(x => x.ApiKey == key);

            return user == null;
        }

        public async Task<IEnumerable<User?>?> GetAllUsersAsync(){
            var allUsers = await database.HashGetAllAsync("Users");

            if(allUsers.Length == 0){
                return null;
            }

            return Array.ConvertAll(allUsers, val => 
                JsonSerializer.Deserialize<User>(val.Value)).ToList();
        }
    }
}