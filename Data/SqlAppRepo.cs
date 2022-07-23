using CarShopAPI.Dtos;
using CarShopAPI.Models;

namespace CarShopAPI.Data{

    public class SqlAppRepo : IAppRepo{
        private readonly AppDbContext _context;

        public SqlAppRepo(AppDbContext context){
            _context = context;
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