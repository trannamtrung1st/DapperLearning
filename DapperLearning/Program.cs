using DapperLearning.Examples;
using System.Text.Json;

//E01_QuickStart.Run();

await E02_QueryData.Run();

static partial class Program
{
    public static JsonSerializerOptions JsonDefault = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}