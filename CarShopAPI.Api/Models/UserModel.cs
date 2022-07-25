using System.ComponentModel.DataAnnotations;

namespace CarShopAPI.Api.Models{

    public class UserModel{
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}