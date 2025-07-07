using System.IO;
using System.Text.Json;
using TestMaster.Models.DB;

namespace TestMaster.Services
{
    public class JsonFileService : IFileService
    {
        private JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = { new QuestionJsonConverter() }
        };

        public void Save(TestDB test, string path)
        {
            var json = JsonSerializer.Serialize(test, options);
            File.WriteAllText(path, json);
        }

        public TestDB Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<TestDB>(json, options);
        }
    }
}