using System.ComponentModel.DataAnnotations;

namespace CarShopAPI.Dtos{

    public class CreateCarModel{        
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