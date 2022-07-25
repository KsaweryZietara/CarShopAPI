using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarShopAPI.Api.Data;
using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;
using CarShopAPI.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarShopAPI.Api.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase{
        private readonly IAppRepo _repository;

        private readonly IConfiguration _configuration;

        public AuthenticateController(IAppRepo repository, IConfiguration configuration){
            _repository = repository;
            _configuration = configuration;
        }

        //POST api/authenticate/register/
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody] UserModel user){
            if(user.Role == "seller" || user.Role == "buyer"){
                
                UserModel registeredUser = await _repository.CreateUserAsync(user);
                
                if(registeredUser != null){
                    await _repository.SaveChangesAsync();
                    return Ok("User created successfully");
                }

                return BadRequest("User already exist");
            }
            return BadRequest("Invalid data");
        }

        //POST api/authenticate/login/
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult LoginUser([FromBody] LoginModel loginModel){
            var user = _repository.FindUser(loginModel);

            if(user == null){
                return Unauthorized();
            }

            var authClaims = new List<Claim>{
                new Claim(ClaimTypes.GivenName, Convert.ToString(user.Id)),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = GetToken(authClaims);

            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims){
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }
}