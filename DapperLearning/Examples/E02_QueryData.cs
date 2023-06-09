﻿using Dapper;
using DapperLearning.Models;
using DapperLearning.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E02_QueryData
    {
        public static async Task Run()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                await QueryScalar(connection);

                await QuerySingleRow(connection);

                await QueryMultipleRows(connection);

                await QueryMultiResults(connection);

                await QuerySpecificColumns(connection);
            }
        }

        static async Task QueryScalar(IDbConnection connection)
        {
            var sql = @"SELECT COUNT(*) FROM production.products";

            var count = await connection.ExecuteScalarAsync<int>(sql);

            Console.WriteLine($"Product count: {count}");
        }

        static async Task QuerySingleRow(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products WHERE product_id=1";

            var entity = await connection.QueryFirstOrDefaultAsync<ProductEntity>(sql);

            DisplayHelper.PrintJson(entity);
        }

        static async Task QueryMultipleRows(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products WHERE model_year=2016";

            var results = await connection.QueryAsync<ProductEntity>(sql);

            DisplayHelper.PrintJson(results);
        }

        static async Task QueryMultiResults(IDbConnection connection)
        {
            var sql = @"
SELECT * FROM production.products WHERE product_id=1;
SELECT * FROM production.products WHERE model_year=2016;";

            using (var multi = await connection.QueryMultipleAsync(sql))
            {
                var entity = await multi.ReadFirstOrDefaultAsync<ProductEntity>();

                var results = await multi.ReadAsync<ProductEntity>();

                DisplayHelper.PrintJson(entity);

                DisplayHelper.PrintJson(results);
            }
        }

        static async Task QuerySpecificColumns(IDbConnection connection)
        {
            var sql = @"SELECT product_id, product_name FROM production.products WHERE product_id=1";

            var entity = await connection.QueryFirstOrDefaultAsync(sql);

            DisplayHelper.PrintJson(entity);
        }
    }
}
