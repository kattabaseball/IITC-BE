using IITCExam.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.BusinessEntity
{
    public interface IUserManagement
    {
        Task<ApiResponseDto<CreateUserResponseDto>> CreateUser(RegisterUserDto newUser);
    }
}
