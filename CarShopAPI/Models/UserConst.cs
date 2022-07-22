namespace CarShopAPI.Models{

    public class UserConst{
        public static List<UserModel> users = new List<UserModel>(){
            new UserModel(){Username = "user", EmailAddress = "user", Password = "user", Role = "Seller"},
            new UserModel(){Username = "admin", EmailAddress = "admin", Password = "admin", Role = "Administrator"}
        };
    }
}