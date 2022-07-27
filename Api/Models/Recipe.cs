using System.ComponentModel.DataAnnotations;

namespace RecipesAPI.Api.Models{

    public class Recipe{
        [Required]
        public string? Id { get; set; }

        [Required]
        public List<string>? Ingredients { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}