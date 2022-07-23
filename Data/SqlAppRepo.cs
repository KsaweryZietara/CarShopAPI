using CarShopAPI.Dtos;
using CarShopAPI.Models;

namespace CarShopAPI.Data{

    public class SqlAppRepo : IAppRepo{
        private readonly AppDbContext _context;

        public SqlAppRepo(AppDbContext context){
            _context = context;
        }

        public IEnumerable<CarModel> CarsByUserId(int id){
            return _context.Cars.Where(x => x.SellerId == id);
        }

        public IEnumerable<CarModel> CarsWithSettings(SearchSettings settings){
            IEnumerable<CarModel> cars = _context.Cars;

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

        public void CreateCar(CarModel car){
            _context.Cars.Add(car);
        }

        public UserModel CreateUser(UserModel user){
            if(user == null){
                throw new ArgumentNullException(nameof(user));
            }
            
            var userExist = _context.Users.FirstOrDefault(x => x.Username == user.Username || 
                x.EmailAddress == user.EmailAddress);
            
            if(userExist == null){
                _context.Users.Add(user);
                return user;
            }

            return null;
        }

        public UserModel FindUser(LoginModel user){
            return _context.Users.FirstOrDefault(x => x.Username == user.Username &&
                x.Password == user.Password);
        }

        public bool SaveChanges(){
            return _context.SaveChanges() >= 0;
        }
    }
}