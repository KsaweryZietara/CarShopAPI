using AutoMapper;
using CarShopAPI.Dtos;
using CarShopAPI.Models;

namespace CarShopAPI.Profiles{

    public class CarsProfile : Profile{
        public CarsProfile(){
            CreateMap<CreateCarModel, CarModel>();
        }
    }
}