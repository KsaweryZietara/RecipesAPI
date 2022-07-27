using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Models;

namespace RecipesAPI.Api.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase{
        private readonly IAppRepo _repo;

        public RecipesController(IAppRepo repo){
            _repo = repo;
        }

        //POST api/recipes/
        [HttpPost]
        public async Task<ActionResult> CreateRecipeAsync([FromBody] Recipe recipe){
            await _repo.CreateRecipeAsync(recipe);
            return Ok("Recipe has been created");
        }

        //GET api/recipes/{id}/
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeByIdAsync([FromRoute] string id){
            var recipe = await _repo.GetRecipeByIdAsync(id);

            if(recipe == null){
                return NotFound();
            }

            return Ok(recipe);
        }

        //GET api/recipes/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes(){
            var allRecipes = await _repo.GetAllRecipesAsync();

            if(allRecipes == null){
                return NotFound();
            }

            return Ok(allRecipes);
        }
    }
}