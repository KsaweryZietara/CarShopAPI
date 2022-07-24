using CarShopAPI.Dtos;
using CarShopAPI.Models;

namespace CarShopAPI.Data{

    public interface IAppRepo{
       bool SaveChanges();

       UserModel CreateUser(UserModel user); 

       UserModel FindUser(LoginModel user);

       void CreateCar(CarModel car);

       IEnumerable<CarModel> CarsByUserId(int id);

       IEnumerable<CarModel> CarsWithSettings(SearchSettings settings);

       CarModel BuyCar(int carId, int buyerId);

       UserModel GetUserById(int id);

       void DeleteUser(UserModel user);

       CarModel GetCarById(int id);

       void DeleteCar(CarModel car);
    }
}