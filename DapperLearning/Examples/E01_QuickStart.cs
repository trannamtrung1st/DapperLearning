using Dapper;
using DapperLearning.Models;
using DapperLearning.Utils;
using Microsoft.Data.SqlClient;

namespace DapperLearning.Examples
{
    public static class E01_QuickStart
    {
        public static void Run()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            IEnumerable<ProductEntity> results;

            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                var sql = @"
SELECT * FROM production.products 
ORDER BY product_id
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY";

                results = connection.Query<ProductEntity>(sql).ToList();
            }

            DisplayHelper.PrintJson(results);
        }
    }
}
