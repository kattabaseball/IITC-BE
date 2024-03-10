using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class CreateUserResponseDto
    {
        public IdentityUser User { get; set; }
    }
}
