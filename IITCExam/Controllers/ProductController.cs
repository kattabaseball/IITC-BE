using IITCExam.Core.BusinessEntity;
using IITCExam.Core.BusinessEntity.Implementation;
using IITCExam.Core.Dto;
using IITCExam.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IITCExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductBusinessEntity _productBusinessEntity { get; set; }

        public ProductController(IProductBusinessEntity productBusinessEntity)
        {
            _productBusinessEntity = productBusinessEntity;
        }

        //get the file and passes it to the business entity layer
        [HttpPost("UploadCSV")]
        public async Task<ResponseDto> UploadCSV(IFormFile file)
        {
            try
            {
                bool result = await this._productBusinessEntity.UploadCsvAsync(file);

                if (result)
                {
                    return new ResponseDto { Status = "Success", Message = "Done" }; 
                }
                else
                {
                    return new ResponseDto { Status = "Failed", Message = "gg" }; 
                }
            }
            catch (Exception ex)
            {
                
                return new ResponseDto { Status = "Error", Message = "Internal Server Error" }; 
            }
        }

        //return all the products
        [HttpPost("GetAllProducts")]

        public PaginatedList<Product> GetAllProducts([FromBody] PaginationDto pagination)
        {
            return _productBusinessEntity.GetAllProducts(pagination);
        }
    }
}
