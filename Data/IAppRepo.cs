using CarShopAPI.Dtos;
using CarShopAPI.Models;

namespace CarShopAPI.Data{

    public interface IAppRepo{
       bool SaveChanges();

       UserModel CreateUser(UserModel user); 

       UserModel FindUser(LoginModel user);

       void CreateCar(CarModel car);
    }
}