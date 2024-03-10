using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class LoginOtpResponseDto
    {
        public string Token { get; set; } = null;
        public IdentityUser User { get; set; } = null;
    }
}
