using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IITCExam.Data.Model
{
    //model for product table
    public class Product
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }     
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public int InStock { get; set; }
    }

    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int ItemsPerPage { get; set; }
        public List<Product> products { get; set; }

    }
}
