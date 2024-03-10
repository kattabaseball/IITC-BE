using Google.Apis.Auth;
using IITCExam.Core.BusinessEntity;
using IITCExam.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IITCExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserManagement _user;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IUserManagement user)
        {
            _user = user;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        //Create user api
        [HttpPost("CreateUser")]
        public async Task<ResponseDto> CreateUser([FromBody] RegisterUserDto newUser)
        {
            var response = await _user.CreateUser(newUser);

            if (response.IsSuccess)
            {

                return new ResponseDto { Status = "Success", Message = response.UserId };

            }

            return new ResponseDto { Status = "Failed", Message = response.Message, Errors = response.Errors };

        }

        //login api
        [HttpPost("login")]

        public async Task<ResponseDto> Login([FromBody] LoginModelDto loginModel)
        {
            //checks for user with username
            var loginUser = await _userManager.FindByNameAsync(loginModel.UserName);
            if (loginUser != null)
            {
                //if the user is there checks if the password is correct
                if (await _userManager.CheckPasswordAsync(loginUser, loginModel.Password))
                {                   
                    //adding data to the claim list to be added to a token 
                    var authClaims = new List<Claim>
                    {
                        new Claim("UserId", loginUser.Id),
                        new Claim("UserName", loginUser.UserName),
                        new Claim("Email", loginUser.Email),
                        new Claim("Mobile", loginUser.PhoneNumber),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    //generate the token with the fields
                    var jwtToken = GetToken(authClaims);

                    var data = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    };


                    return new ResponseDto { Status = "Success", Message = "Login Successful", Data = data };

                }

                return new ResponseDto { Status = "Failed", Message = "Invalid username or password" };
        }

            return new ResponseDto { Status = "Failed", Message = "Invalid username or password" };

        }

        //generating the token
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;

        }

        //login with google api
        [HttpPost("LoginWithGoogle/{credential}")]
        public async Task<ResponseDto> LoginWithGoogle(string credential)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings();
            {
                var Audience = new List<string> { "1008681447232-u9lf637ejn2gftt00n3b2u88kef36ll0.apps.googleusercontent.com" };

            };   

            var payLoad = await GoogleJsonWebSignature.ValidateAsync(credential, settings);  
            
            var user = await _userManager.FindByEmailAsync(payLoad.Email);  

            if(user != null)
            {
                //adding data to the claim list to be added to a token 
                var authClaims = new List<Claim>
                    {
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email),
                        new Claim("Mobile", user.PhoneNumber),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                //generate the token with the fields
                var jwtToken = GetToken(authClaims);

                var data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                };


                return new ResponseDto { Status = "Success", Message = "Login Successful", Data = data };
            }
            else
            {

                return new ResponseDto { Status = "Failed", Message = "Login Unsuccessful"};
            }

                

































        }



    }
}
