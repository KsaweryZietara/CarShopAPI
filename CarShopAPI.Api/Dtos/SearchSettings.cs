namespace CarShopAPI.Api.Dtos{

    public class SearchSettings{
        public string? Make { get; set; }

        public string? Model { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public int? MinManufactureYear  { get; set; }

        public int? MaxManufactureYear { get; set; }
    }
}