﻿using Dapper;
using DapperLearning.Models;
using DapperLearning.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E07_Parameters
    {
        public static async Task Run()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                await SqlInjection(connection);

                await AnonymousParameters(connection);

                await DynamicParameters(connection);

                await StringParameters(connection);

                await WhereInParameters(connection);

                await OutputParameters(connection);
            }
        }

        static async Task SqlInjection(IDbConnection connection)
        {
            try
            {
                Console.Write("Search products: ");
                var search = Console.ReadLine();

                var sql = @$"SELECT * FROM production.products WHERE product_name LIKE '%{search}%'";

                var products = await connection.QueryAsync<ProductEntity>(sql);

                DisplayHelper.PrintJson(products);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        static async Task AnonymousParameters(IDbConnection connection)
        {
            var search = "hello; SELECT * FROM dbo.users;";

            var sql = @"
SELECT * FROM production.products 
WHERE product_name LIKE @Search OR model_year = @Year";

            var products = await connection.QueryAsync<ProductEntity>(sql, new
            {
                Search = $"%{search}%",
                Year = 2016
            });

            DisplayHelper.PrintJson(products);
        }

        static async Task DynamicParameters(IDbConnection connection)
        {
            var dynamicParameters = new DynamicParameters(new { ProductId = 1 });

            dynamicParameters.AddDynamicParams(new { NameContains = "%tele%", CategoryId = 2 });

            dynamicParameters.Add("@NameEquals", "Television", DbType.String, ParameterDirection.Input, 10);

            var sql = @"
SELECT * FROM production.products 
WHERE product_name LIKE @NameContains
    OR product_name = @NameEquals
    OR category_id = @CategoryId
    OR product_id = @ProductId;";

            var products = await connection.QueryAsync<ProductEntity>(sql, dynamicParameters);

            DisplayHelper.PrintJson(products);
        }

        static async Task StringParameters(IDbConnection connection)
        {
            string sql = @"SELECT * FROM production.products WHERE product_name LIKE @Name";

            var dbParams = new DbString()
            {
                Value = "%Trek %",
                IsAnsi = true,
                IsFixedLength = true,
                Length = 7
            };

            var firstProduct = await connection.QueryFirstOrDefaultAsync<ProductEntity>(sql,
                new
                {
                    Name = dbParams
                });

            DisplayHelper.PrintJson(firstProduct);
        }

        static async Task WhereInParameters(IDbConnection connection)
        {
            string sql = @"SELECT * FROM production.products WHERE product_id IN @Ids";

            var products = await connection.QueryAsync<ProductEntity>(sql,
                new
                {
                    Ids = new[] { 1, 2, 4, 5 }
                });

            DisplayHelper.PrintJson(products);
        }

        static async Task OutputParameters(IDbConnection connection)
        {
            const string ProcName = "GetProductDetails";
            string createProcSql = @$"
CREATE OR ALTER PROC {ProcName}
   @ProductId          INT,
   @Name               NVARCHAR(Max)         OUTPUT,
   @ModelYear          INT                   OUTPUT
AS
   SELECT
      @Name=product_name,
      @ModelYear=model_year FROM production.products
   WHERE product_id=@ProductId
";

            await connection.ExecuteAsync(createProcSql);

            var parameters = new DynamicParameters(new
            {
                ProductId = 1
            });
            parameters.Add("@Name", null, dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
            parameters.Add("@ModelYear", null, dbType: DbType.Int16, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(ProcName, parameters, commandType: CommandType.StoredProcedure);

            var name = parameters.Get<string>("@Name");
            var modelYear = parameters.Get<short>("@ModelYear");

            Console.WriteLine($"{name} - {modelYear}");
        }
    }
}
