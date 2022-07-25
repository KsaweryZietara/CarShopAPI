using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;

namespace CarShopAPI.Api.Data{

    public interface IAppRepo{
       Task<bool> SaveChangesAsync();

       Task<UserModel> CreateUserAsync(UserModel user); 

       UserModel FindUser(LoginModel user);

       Task CreateCarAsync(CarModel car);

       IEnumerable<CarModel> CarsByUserId(int id);

       IEnumerable<CarModel> CarsWithSettings(SearchSettings settings);

       Task<CarModel> BuyCarAsync(int carId, int buyerId);

       Task<UserModel> GetUserByIdAsync(int id);

       void DeleteUser(UserModel user);

       Task<CarModel> GetCarByIdAsync(int id);

       void DeleteCar(CarModel car);
    }
}