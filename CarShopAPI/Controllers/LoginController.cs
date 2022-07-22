using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarShopAPI.Dtos;
using CarShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarShopAPI.Controllers{
    
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase{
        
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config){
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IResult Login([FromBody] UserLoginDto userLoginDto){
            var user = Authenticate(userLoginDto);

            if(user != null){
                var token = Generate(user);
                return Results.Ok(token);
            }

            return Results.NotFound("User not found");
        }

        private string Generate(UserModel user){
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLoginDto userLoginDto){
            var currentUser = UserConst.users.FirstOrDefault(x => x.Username == userLoginDto.Username &&
                x.Password == userLoginDto.Password);
            
            if(currentUser != null){
                return currentUser;
            }

            return null;
        }
    }
}