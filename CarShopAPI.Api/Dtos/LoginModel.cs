using System.ComponentModel.DataAnnotations;

namespace CarShopAPI.Api.Dtos{
    public class LoginModel{
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}