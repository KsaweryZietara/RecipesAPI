using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Models;


namespace RecipesAPI.Api.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase{
        private readonly IAppRepo _repo;

        private readonly ActivitySource _myActivitySource;

        public RecipesController(IAppRepo repo, ActivitySource myActivitySource){
            _repo = repo;
            _myActivitySource = myActivitySource;
        }

        //POST api/recipes/
        [HttpPost]
        public async Task<ActionResult> CreateRecipeAsync([FromBody] Recipe recipe){
            using var activity = _myActivitySource.StartActivity("CreateRecipeAsync");
            
            await _repo.CreateRecipeAsync(recipe);
            return Ok("Recipe has been created");
        }

        //GET api/recipes/{id}/
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeByIdAsync([FromRoute] string id){
            using var activity = _myActivitySource.StartActivity($"GetRecipesByIdAsync: {id}");
            
            var recipe = await _repo.GetRecipeByIdAsync(id);

            if(recipe == null){
                return NotFound();
            }

            return Ok(recipe);
        }

        //GET api/recipes/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipesAsync(){
            using var activity = _myActivitySource.StartActivity("GetAllRecipesAsync");

            var allRecipes = await _repo.GetAllRecipesAsync();

            if(allRecipes == null){
                return NotFound();
            }

            return Ok(allRecipes);
        }
    }
}