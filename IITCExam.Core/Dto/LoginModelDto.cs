using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }
        public string? UserName { get; set; }
    }
}
