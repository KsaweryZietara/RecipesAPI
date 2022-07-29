using System.Diagnostics;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecipesAPI.Api.Controllers;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Models;
using Xunit;

namespace RecipesAPI.UnitTests{

    public class RecipesControllerTests{
        private readonly Mock<IAppRepo> respositoryStub = new Mock<IAppRepo>();

        private readonly ActivitySource activitySourceStub = new ActivitySource("test");
        
        [Fact]
        public async Task CreateRecipeAsync_RecipeHasBeenCreated_ReturnsOk(){
            //Arrange
            var controller = new RecipesController(respositoryStub.Object, activitySourceStub);
        
            //Act
            var result = await controller.CreateRecipeAsync(new Recipe());
            
            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetRecipeByIdAsync_RecipeIsNull_ReturnsNull(){
            //Arrange
            respositoryStub.Setup(repo => repo.GetRecipeByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync((Recipe)null);
        
            var controller = new RecipesController(respositoryStub.Object, activitySourceStub);
        
            //Act
            var result = await controller.GetRecipeByIdAsync("test");
            
            //Assert
            var resultRecipe = (result.Result as ObjectResult);
            resultRecipe.Should().BeNull();
        }

        [Fact]
        public async Task GetRecipeByIdAsync_RecipeIsValid_ReturnsRecipe(){
            //Arrange
            var recipe = RandomRecipe();
            respositoryStub.Setup(repo => repo.GetRecipeByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync(recipe);
        
            var controller = new RecipesController(respositoryStub.Object, activitySourceStub);
        
            //Act
            var result = await controller.GetRecipeByIdAsync("test");
            
            //Assert
            var resultRecipe = (result.Result as ObjectResult).Value as Recipe;
            resultRecipe.Should().BeEquivalentTo(recipe, options => 
                options.ComparingByMembers<Recipe>());
        }

        [Fact]
        public async Task GetAllRecipesAsync_AllRecipesAreNull_ReturnsNull(){
            //Arrange
            respositoryStub.Setup(repo => repo.GetAllRecipesAsync())
                            .ReturnsAsync((IEnumerable<Recipe>)null);

            var controller = new RecipesController(respositoryStub.Object, activitySourceStub);
        
            //Act
            var result = await controller.GetAllRecipesAsync();
            
            //Assert
            var resultRecipe = (result.Result as ObjectResult);
            resultRecipe.Should().BeNull();
        }

        [Fact]
        public async Task GetAllRecipesAsync_AllRecipesAreValid_ReturnsAllRecipes(){
            //Arrange
            Recipe[] recipes = { RandomRecipe(), RandomRecipe(), RandomRecipe() };
            respositoryStub.Setup(repo => repo.GetAllRecipesAsync())
                            .ReturnsAsync(recipes);
        
            var controller = new RecipesController(respositoryStub.Object, activitySourceStub);
        
            //Act
            var result = await controller.GetAllRecipesAsync();
            
            //Assert
            var resultRecipe = (result.Result as ObjectResult).Value as IEnumerable<Recipe>;
            resultRecipe.Should().BeEquivalentTo(recipes, options => 
                options.ComparingByMembers<Recipe>());
        }

        private Recipe RandomRecipe(){
            var recipe = new Recipe(){
                Id = Guid.NewGuid().ToString(),
                Ingredients = new List<string>(){Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                Description = Guid.NewGuid().ToString()
            };

            return recipe;
        }
    }
}