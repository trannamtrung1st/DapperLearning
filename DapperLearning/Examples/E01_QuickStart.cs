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

            var connectionString = "Server=localhost,1434;Database=BikeStores;Trusted_Connection=False;User Id=sa;Password=z@123456!;MultipleActiveResultSets=true;TrustServerCertificate=True";

            IEnumerable<ProductEntity> results;

            using (var connection = new SqlConnection(connectionString))
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
