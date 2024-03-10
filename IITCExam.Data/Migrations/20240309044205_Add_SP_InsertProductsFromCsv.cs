using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IITCExam.Data.Migrations
{
    public partial class Add_SP_InsertProductsFromCsv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = $@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertProductsFromCsv]') AND type in (N'P', N'PC'))
                BEGIN
                EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertProductsFromCsv] AS' 
                END
                GO

                -- =============================================
				-- Author:		<Author - Kavindu>
				-- Create date: <Create Date,2024.03.09,>
				-- Description:	<Adding csv data to the product table,>
				-- =============================================
				CREATE OR ALTER PROCEDURE [dbo].[InsertProductsFromCsv]
                @Products dbo.ProductType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Insert data from the table-valued parameter into the Products table
                    INSERT INTO dbo.Product (Name, Description, Quantity, Amount, InStock)
                    SELECT Name, Description, Quantity, Amount, InStock
                    FROM @Products;
                END


                ";

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = $@"DROP PROCEDURE IF EXISTS [dbo].[InsertProductsFromCsv]";
            migrationBuilder.Sql(sql);
        }
    }
}
