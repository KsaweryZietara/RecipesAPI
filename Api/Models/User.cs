using System.ComponentModel.DataAnnotations;

namespace RecipesAPI.Api.Models{

    public class User{
        [Required]
        public string? ApiKey { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}