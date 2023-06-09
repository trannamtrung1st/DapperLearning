﻿using Dapper;
using DapperLearning.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E04_ExecuteCommand
    {
        public static async Task Run()
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                var insertedId = await Insert(connection);

                await Update(connection, insertedId);

                await MultipleCommands(connection);
            }
        }

        static async Task<int> Insert(IDbConnection connection)
        {
            var sql = @"
INSERT INTO production.products
    (product_name, brand_id, category_id, model_year, list_price)
OUTPUT inserted.product_id
VALUES 
    (@ProductName, @BrandId, @CategoryId, @ModelYear, @ListPrice);";

            var id = await connection.ExecuteScalarAsync<int>(sql, new ProductEntity
            {
                BrandId = 1,
                CategoryId = 1,
                ListPrice = 150,
                ModelYear = 2023,
                ProductName = "My 2023 Product"
            });

            Console.WriteLine($"New product ID: {id}");

            return id;
        }

        static async Task Update(IDbConnection connection, int insertedId)
        {
            var sql = @"
UPDATE production.products SET 
    product_name=@ProductName,
    model_year=@ModelYear
WHERE product_id=@ProductId;";

            var count = await connection.ExecuteAsync(sql, new
            {
                ProductId = insertedId,
                ProductName = "My new product 2023",
                ModelYear = 2023
            });

            Console.WriteLine($"Updated count: {count}");
        }

        static async Task MultipleCommands(IDbConnection connection)
        {
            var sql = @"
UPDATE production.products SET 
    product_name=''
WHERE model_year=@ModelYear;

DELETE FROM production.products
WHERE model_year=@ModelYear;";

            var count = await connection.ExecuteAsync(sql, new
            {
                ModelYear = 2023
            });

            Console.WriteLine($"Affected rows: {count}");
        }
    }
}
