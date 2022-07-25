using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using CarShopAPI.Api.Security;

namespace CarShopAPI.Api.Data{

    public class SqlAppRepo : IAppRepo{
        private readonly AppDbContext _context;

        public SqlAppRepo(AppDbContext context){
            _context = context;
        }

        public async Task<CarModel> BuyCarAsync(int carId, int buyerId){
            var car = await _context.Cars.Where(x => x.Id == carId && x.BuyerId == null)
                                    .FirstOrDefaultAsync();
            
            if(car != null){
                car.BuyerId = buyerId;  
                car.Status = "Finalized";
                return car;              
            }       

            return null;
        }

        public IEnumerable<CarModel> CarsByUserId(int id){
            return _context.Cars.Where(x => x.SellerId == id);
        }

        public IEnumerable<CarModel> CarsWithSettings(SearchSettings settings){
            IEnumerable<CarModel> cars = _context.Cars.Where(x => x.Status == "available");

            if(settings.Make != null){
                cars = cars.Where(x => x.Make == settings.Make);
            } 

            if(settings.Model != null){
                cars = cars.Where(x => x.Model == settings.Model);
            }

            if(settings.MinPrice != null){
                cars = cars.Where(x => x.Price > settings.MinPrice);
            }

            if(settings.MaxPrice != null){
                cars = cars.Where(x => x.Price < settings.MaxPrice);
            }

            if(settings.MinManufactureYear != null){
                cars = cars.Where(x => x.ManufactureYear > settings.MinManufactureYear);
            }

            if(settings.MaxManufactureYear != null){
                cars = cars.Where(x => x.ManufactureYear < settings.MaxManufactureYear);
            }

            return cars;
        }

        public async Task CreateCarAsync(CarModel car){
            await _context.Cars.AddAsync(car);
        }

        public async Task<UserModel> CreateUserAsync(UserModel user){ 
            var userExist = await _context.Users.FirstOrDefaultAsync(x => x.Username == user.Username || 
                x.EmailAddress == user.EmailAddress);
            
            if(userExist == null){
                user.Password = user.Password.PasswordHashing();
                await _context.Users.AddAsync(user);
                return user;
            }

            return null;
        }

        public void DeleteCar(CarModel car){
            _context.Cars.Remove(car);
        }

        public void DeleteUser(UserModel user){
            _context.Users.Remove(user);
        }

        public UserModel FindUser(LoginModel user){
            foreach(UserModel u in _context.Users){
                if(user.Username == u.Username && user.Password.CheckPassword(u.Password)){
                    return u;
                }
            }
            
            return null;
        }

        public async Task<CarModel> GetCarByIdAsync(int id){
            return await _context.Cars.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id){
            return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync(){
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}