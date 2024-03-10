using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class ApiResponseDto<T>
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public int StatusCode { get; set; }

        public T? Response { get; set; }

        public List<String> Errors { get; set; }

        public string UserId { get; set; }
    }
}
