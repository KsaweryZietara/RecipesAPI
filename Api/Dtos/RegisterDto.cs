using System.ComponentModel.DataAnnotations;

namespace RecipesAPI.Api.Dtos{

    public class RegisterDto{
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}