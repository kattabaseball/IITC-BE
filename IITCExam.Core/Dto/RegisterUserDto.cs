﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string? UserName { get; set; }

        [Phone]
        [Required(ErrorMessage = "Contact Number is Required")]
        public string? ContactNumber { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }    
        

        [Required(ErrorMessage = "Password  is Required")]
        public string? Password { get; set; }
    }
}
