using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarShopAPI.Models{

    public class CarModel{
        [Key]
        public int Id { get; set; }

        [Required]
        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

        [Required]
        public string? Status { get; set; }
        
        [Required]
        public string? Make { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int ManufactureYear  { get; set; }
    }
}