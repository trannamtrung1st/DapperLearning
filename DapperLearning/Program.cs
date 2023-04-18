using DapperLearning.Examples;
using System.Text.Json;

//E01_QuickStart.Run();

//await E02_QueryData.Run();

//await E03_MappingConfig.Run();

//await E04_ExecuteCommand.Run();

//await E05_ExecuteReader.Run();

//await E06_Relationships.Run();

await E07_Parameters.Run();

static partial class Program
{
    public const string ConnectionString = "Server=localhost,1434;Database=BikeStores;Trusted_Connection=False;User Id=sa;Password=z@123456!;MultipleActiveResultSets=true;TrustServerCertificate=True";

    public static JsonSerializerOptions JsonDefault = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}