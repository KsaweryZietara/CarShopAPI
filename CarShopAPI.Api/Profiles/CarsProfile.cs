using AutoMapper;
using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;

namespace CarShopAPI.Api.Profiles{

    public class CarsProfile : Profile{
        public CarsProfile(){
            CreateMap<CreateCarModel, CarModel>();
        }
    }
}