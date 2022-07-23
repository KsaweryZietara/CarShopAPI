using System.Security.Claims;
using AutoMapper;
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

        private readonly IMapper _mapper;

        public UserController(IAppRepo repository, IMapper mapper){
            _repository = repository;
            _mapper = mapper;
        }

        //POST api/user/newcar
        [Authorize(Roles = "seller")]
        [HttpPost]
        [Route("newcar")]
        public IActionResult NewCar([FromBody] CreateCarModel createCarModel){
            CarModel car = _mapper.Map<CarModel>(createCarModel);
            car.Status = "available";
            
            IEnumerable<Claim> claims = GetClaims();
            
            car.SellerId = claims.Where(x => x.Type == ClaimTypes.GivenName)
                            .Select(x => Convert.ToInt32(x.Value))
                            .FirstOrDefault();
            
            _repository.CreateCar(car);
            _repository.SaveChanges();

            return Ok("Car put up for sale successfully");
        }
        
        //GET api/user/carsforsale
        [Authorize(Roles = "seller")]
        [HttpGet]
        [Route("carsforsale")]
        public ActionResult<IEnumerable<CarModel>> CarsForSale(){
            int sellerId = GetClaims().Where(x => x.Type == ClaimTypes.GivenName)
                                        .Select(x => Convert.ToInt32(x.Value))
                                        .FirstOrDefault();

            return Ok(_repository.CarsByUserId(sellerId));
        }

        [Authorize(Roles = "buyer,seller")]
        [HttpGet]
        [Route("carswithsearchsettings")]
        public ActionResult<IEnumerable<CarModel>> Search([FromBody] SearchSettings searchSettings){
            return Ok(_repository.CarsWithSettings(searchSettings));
        }

        private IEnumerable<Claim> GetClaims(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims;
        }
    }
}