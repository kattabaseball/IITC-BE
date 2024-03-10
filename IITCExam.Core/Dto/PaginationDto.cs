using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.Dto
{
    public class PaginationDto
    {
        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public string? SearchText { get; set; }
        
    }
}
