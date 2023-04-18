using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E05_ExecuteReader
    {
        public static async Task Run()
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                await QueryProducts(connection);
            }
        }

        static async Task QueryProducts(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products";

            var dataReader = await connection.ExecuteReaderAsync(sql);

            var datatable = new DataTable();
            datatable.Load(dataReader);
        }
    }
}
