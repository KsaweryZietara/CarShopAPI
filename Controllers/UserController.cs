using CarShopAPI.Data;
using CarShopAPI.Dtos;
using CarShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarShopAPI.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase{
        private readonly IAppRepo _repository;

        public UserController(IAppRepo repository){
            _repository = repository;
        }

        //POST api/user/newcar
        [Authorize(Roles = "seller")]
        [HttpPost]
        [Route("newcar")]
        public IActionResult NewCar([FromBody] CreateCarModel createCarModel){
            //maping

            
        }

    }
}