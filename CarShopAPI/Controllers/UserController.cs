using System.Security.Claims;
using CarShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarShopAPI.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase{

        [HttpGet("Admins")]
        public IResult AdminsEndpoint(){
            var currentUser = GetCurrentUser();

            return Results.Ok($"HI {currentUser.Role}");
        }

        private UserModel GetCurrentUser(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
    
            if(identity != null){
                var userClaims = identity.Claims;

                return new UserModel{
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }
    }
}