using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Dtos;
using RecipesAPI.Api.Models;

namespace RecipesAPI.Api.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase{
        private readonly IAppRepo _repo;

        private readonly ActivitySource _myActivitySource;

        public UserController(IAppRepo repo, ActivitySource myActivitySource){
            _repo = repo;
            _myActivitySource = myActivitySource;
        }

        //POST api/user/
        [HttpPost]
        public async Task<ActionResult> CreateUserAsync([FromBody] RegisterDto registerDto){
            using var activity = _myActivitySource.StartActivity("CreateUserAsync");
            
            var createdUser = await _repo.CreateUserAsync(registerDto);

            if(createdUser == null){
                return BadRequest("User already exist.");
            }

            return Ok($"Your api key: {createdUser.ApiKey}");
        }

        //POST api/user/getkey/
        [HttpPost("getkey")]
        public async Task<ActionResult<string>> GetUserKeyAsync([FromBody] LoginDto loginDto){
            using var activity = _myActivitySource.StartActivity("GetUserKeyAsync");

            string? userKey = await _repo.GetUserKeyAsync(loginDto);

            if(userKey == null){
                return NotFound();
            }

            return Ok($"Your api key: {userKey}");
        }
    }
}