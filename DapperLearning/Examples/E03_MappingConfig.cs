using Dapper;
using DapperLearning.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearning.Examples
{
    public static class E03_MappingConfig
    {
        public static async Task Run()
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                await QueryDefault(connection);

                await QueryMatchingUnderscores(connection);

                await QueryCustomMapping(connection);
            }
        }

        #region Default
        class DefaultProductModel
        {
            public int ProductId { get; set; }
            public int product_id { get; set; }
        }

        static async Task QueryDefault(IDbConnection connection)
        {
            var sql = @"SELECT * FROM production.products";

            var products = await connection.QueryAsync<DefaultProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion

        #region Matching underscores
        class MatchingUnderscoresProductModel
        {
            public int ProductId { get; set; }
        }

        static async Task QueryMatchingUnderscores(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var sql = @"SELECT * FROM production.products";

            var products = await connection.QueryAsync<MatchingUnderscoresProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion

        #region Custom mapping
        class CustomMappingProductModel
        {
            public int CustomProductId { get; set; }
        }

        static async Task QueryCustomMapping(IDbConnection connection)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = false;

            var customMap = new CustomPropertyTypeMap(
                typeof(CustomMappingProductModel),
                (type, columnName) =>
                {
                    if (columnName == "product_id")
                    {
                        return type.GetProperty(nameof(CustomMappingProductModel.CustomProductId));
                    }

                    return null;
                }
            );

            Dapper.SqlMapper.SetTypeMap(typeof(CustomMappingProductModel), customMap);

            var sql = @"SELECT * FROM production.products";

            var products = await connection.QueryAsync<CustomMappingProductModel>(sql);

            DisplayHelper.PrintJson(products);
        }
        #endregion
    }
}
