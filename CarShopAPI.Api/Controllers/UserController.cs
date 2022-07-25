using System.Security.Claims;
using AutoMapper;
using CarShopAPI.Api.Data;
using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarShopAPI.Api.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase{
        private readonly IAppRepo _repository;

        private readonly IMapper _mapper;

        public UserController(IAppRepo repository, IMapper mapper){
            _repository = repository;
            _mapper = mapper;
        }

        //POST api/user/newcar/
        [Authorize(Roles = "seller")]
        [HttpPost]
        [Route("newcar")]
        public async Task<IActionResult> NewCarAsync([FromBody] CreateCarModel createCarModel){
            CarModel car = _mapper.Map<CarModel>(createCarModel);
            car.Status = "available";
            
            IEnumerable<Claim> claims = GetClaims();
            
            car.SellerId = claims.Where(x => x.Type == ClaimTypes.GivenName)
                            .Select(x => Convert.ToInt32(x.Value))
                            .FirstOrDefault();
            
            await _repository.CreateCarAsync(car);
            await _repository.SaveChangesAsync();

            return Ok("Car put up for sale successfully");
        }
        
        //GET api/user/carsforsale/
        [Authorize(Roles = "seller")]
        [HttpGet]
        [Route("carsforsale")]
        public ActionResult<IEnumerable<CarModel>> CarsForSale(){
            int sellerId = GetClaims().Where(x => x.Type == ClaimTypes.GivenName)
                                        .Select(x => Convert.ToInt32(x.Value))
                                        .FirstOrDefault();

            return Ok(_repository.CarsByUserId(sellerId));
        }

        //GET api/user/carswithsearchsettings/
        [Authorize(Roles = "buyer,seller")]
        [HttpGet]
        [Route("carswithsearchsettings")]
        public ActionResult<IEnumerable<CarModel>> Search([FromBody] SearchSettings searchSettings){
            return Ok(_repository.CarsWithSettings(searchSettings));
        }

        //PUT api/user/buycar/{id}/
        [Authorize(Roles = "buyer")]
        [HttpPut("buycar/{id}")]
        public async Task<ActionResult> BuyCarAsync([FromRoute] int id){
            int buyerId = GetClaims().Where(x => x.Type == ClaimTypes.GivenName)
                                        .Select(x => Convert.ToInt32(x.Value))
                                        .FirstOrDefault();
            
            var car = await _repository.BuyCarAsync(id, buyerId);
            await _repository.SaveChangesAsync();

            if(car != null){
                return Ok("The purchase was successful");
            }   

            return NotFound();    
        }   

        //DELETE api/user/deleteuser/{id}/
        [Authorize(Roles = "admin")]
        [HttpDelete("deleteuser/{id}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] int id){
            var user = await _repository.GetUserByIdAsync(id);

            if(user == null){
                return NotFound();
            }

            _repository.DeleteUser(user);
            await _repository.SaveChangesAsync();

            return Ok("User has been deleted");
        }

        //DELETE api/user/deletecar/{id}/
        [Authorize(Roles = "admin")]
        [HttpDelete("deletecar/{id}")]
        public async Task<ActionResult> DeleteCarAsync([FromRoute] int id){
            var car = await _repository.GetCarByIdAsync(id);

            if(car == null){
                return NotFound();
            }

            _repository.DeleteCar(car);
            await _repository.SaveChangesAsync();

            return Ok("Car has been deleted");
        }

        private IEnumerable<Claim> GetClaims(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims;
        }
    }
}