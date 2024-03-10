using IITCExam.Core.Dto;
using IITCExam.Data.DataService;
using IITCExam.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Core.BusinessEntity.Implementation
{
    public class ProductBusinessEntity : BaseBusinessEntity, IProductBusinessEntity
    {
        IProductDataService _productDataService;
        public ProductBusinessEntity(ILogger<BaseBusinessEntity> logger, IProductDataService productDataService) : base(logger)
        {
            _productDataService = productDataService;   
        }

        //passes only the releven data to the data access layer
        public PaginatedList<Product> GetAllProducts(PaginationDto pagination)
        {
            return _productDataService.GetAllProducts(pagination.SearchText ,pagination.PageNumber, pagination.ItemsPerPage);
        }


        public async Task<bool> UploadCsvAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false; // Handle empty file
            }

            // check for the csv extention
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return false; // Handle non-CSV file
            }

            // read the CSV file
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var dataList = new List<string[]>();
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');
                    dataList.Add(values);
                }

                // passing the list to the data access layer to be added to the table
                bool success = await _productDataService.AddDataFromCsv(dataList);

                return success;
            }
        }

        
    }
}
