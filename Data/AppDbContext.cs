using CarShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShopAPI.Data{

    public class AppDbContext : DbContext{
        public DbSet<CarModel> Cars => Set<CarModel>();

        public DbSet<UserModel> Users => Set<UserModel>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        }
    }
}