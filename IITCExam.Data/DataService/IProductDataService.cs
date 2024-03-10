using IITCExam.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Data.DataService
{
    public interface IProductDataService
    {
        //this is the interface for Product data service and this make the code more readable since only the implementation is on the data service layer
        Task<bool> AddDataFromCsv(List<string[]> dataList);
        PaginatedList<Product> GetAllProducts(string searchText ,int pageNumber, int itemsPerPage);
    }
}
