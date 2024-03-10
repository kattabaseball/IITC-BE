using IITCExam.Core.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.BusinessEntity.Implementation
{
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private string modifiedUserName;

        public UserManagement(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        //this method will check if the user exists with the same email or mobile number if it exists it will return an error
        //if the username contains two words only the first one will taken to be saved in the asp net user table
        public async Task<ApiResponseDto<CreateUserResponseDto>> CreateUser(RegisterUserDto newUser)
        {
            var userExist = await _userManager.FindByEmailAsync(newUser.Email);

            var userWithSameMobile = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == newUser.ContactNumber);

            if (userExist != null)
            {
                return new ApiResponseDto<CreateUserResponseDto> { IsSuccess = false, StatusCode = 403, Message = "User already exists with the same email!" };
            }
            if (userWithSameMobile != null)
            {
                return new ApiResponseDto<CreateUserResponseDto> { IsSuccess = false, StatusCode = 403, Message = "User already exists with the same mobile number!" };
            }

            if (newUser.UserName.Contains(" "))
            {
                // Split the string by space and take the first part
                string[] parts = newUser.UserName.Split(' ');
                modifiedUserName = parts[0];
            }
            else
            {
                modifiedUserName = newUser.UserName;
            }
            //creating identity user
            IdentityUser user = new()
            {
                Email = newUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = modifiedUserName,
                PhoneNumber = newUser.ContactNumber
            };

            var createUserResult = await _userManager.CreateAsync(user, newUser.Password);

            //if the user creation is not a success this will return a list of errors
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors;
                var errorMessages = new List<string>();

                foreach (var error in errors)
                {
                    errorMessages.Add($"Code: {error.Code}, Description: {error.Description}");
                }

                return new ApiResponseDto<CreateUserResponseDto> { IsSuccess = false, StatusCode = 500, Message = "User failed to create!", Errors = errorMessages };
            }

            return new ApiResponseDto<CreateUserResponseDto> { Response = new CreateUserResponseDto() { User = user}, IsSuccess = true, StatusCode = 200, Message = "Account created!", UserId = user.Id };

        }

     
        }

    
}
