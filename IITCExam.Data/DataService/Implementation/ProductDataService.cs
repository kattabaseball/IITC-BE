using IITCExam.Data.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace IITCExam.Data.DataService.Implementation
{
    public class ProductDataService : BaseDataService<Product>, IProductDataService
    {
        public IConfiguration Configuration { get; set; }
        public ProductDataService(DatabaseContext databaseContext, IConfiguration configuration) : base(databaseContext)
        {
            this.Configuration = configuration;
        }

        //in here the data will be chopped into batches of 1000 and will be sent to the backend stored procedure


        //Run this query in the table before excecuting the stored procedure cause this table type is needed when storing data to the table

        //CREATE TYPE ProductType AS TABLE
        //(
        //        Name NVARCHAR(100),
        //        Description NVARCHAR(255),
        //        Quantity INT,
        //        Amount DECIMAL(18, 2),
        //        InStock INT
        //    );


        public async Task<bool> AddDataFromCsv(List<string[]> dataList)
        {
            try
            {
                int batchSize = 1000; // Define the batch size
                for (int i = 0; i < dataList.Count; i += batchSize)
                {
                    var batch = dataList.Skip(i).Take(batchSize).ToList();
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.AddRange(new DataColumn[]
                    {
                        new DataColumn("Name", typeof(string)),
                        new DataColumn("Description", typeof(string)),
                        new DataColumn("Quantity", typeof(int)),
                        new DataColumn("Amount", typeof(decimal)),
                        new DataColumn("InStock", typeof(int))
                    });

                    foreach (var data in batch)
                    {
                        dataTable.Rows.Add(data);
                    }
                    //connecting with the db after getting the connection string from app settings and calling the sp
                    using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                    {
                        await connection.OpenAsync();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "InsertProductsFromCsv";
                            command.Parameters.AddWithValue("@Products", dataTable);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //getting all te products
        public PaginatedList<Product> GetAllProducts(string? searchText, int pageNumber, int itemsPerPage)
        {
            //using IQueryable performs better cause it can be filters in the db side
            IQueryable<Product> query = databaseContext.Product;

            //checks for similar data with search text
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText) 
                || p.Quantity.ToString() == searchText || p.Amount.ToString() == searchText || p.InStock.ToString() == searchText);
            }

            //total data count taken
            //total pages will be taken based on the items per page currently im keeping 30 per page
            // then the 30 data rows in the page before the current page will be skippedand will taken the next 30 and order by id descending

            var totalCount = databaseContext.Product.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / itemsPerPage);
            var productsPerPage = query.Skip((pageNumber-1)* itemsPerPage).Take(itemsPerPage).OrderByDescending(x=>x.Id).ToList();

            //paginated products dto will be created and returned with the relevent data

            var products = new PaginatedList<Product>()
            {
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                ItemsPerPage = itemsPerPage,
                products = productsPerPage
            };

            return products;
        }
    }
}
