using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E08_Others
    {
        public static async Task Run()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                await Transaction(connection);
            }
        }

        static async Task Transaction(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @$"DELETE FROM production.products WHERE product_id = @Id";

                await connection.ExecuteAsync(sql, new
                {
                    Id = 1000
                }, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                transaction.Rollback();
            }
        }
    }
}
