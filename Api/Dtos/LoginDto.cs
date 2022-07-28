using System.ComponentModel.DataAnnotations;

namespace RecipesAPI.Api.Dtos{

    public class LoginDto{
        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}