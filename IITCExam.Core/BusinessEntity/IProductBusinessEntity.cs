using IITCExam.Core.Dto;
using IITCExam.Data.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.BusinessEntity
{
    public interface IProductBusinessEntity
    {
        //this is the interface for Product business layer and this make the code more readable since only the implementation is on the business logic layer
        PaginatedList<Product> GetAllProducts(PaginationDto pagination);
        Task<bool> UploadCsvAsync(IFormFile file);
    }
}
