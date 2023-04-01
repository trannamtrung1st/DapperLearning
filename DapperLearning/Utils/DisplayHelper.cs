using System.Text.Json;

namespace DapperLearning.Utils
{
    public static class DisplayHelper
    {
        public static void PrintJson(object value)
        {
            Console.WriteLine(JsonSerializer.Serialize(value, Program.JsonDefault));
        }
    }
}
