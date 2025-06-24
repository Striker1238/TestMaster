using System.IO;
using System.Text.Json;
using TestMaster.Models;

namespace TestMaster.Services
{
    public class JsonFileService : IFileService
    {
        private JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = { new QuestionJsonConverter() }
        };

        public void Save(Test test, string path)
        {
            var json = JsonSerializer.Serialize(test, options);
            File.WriteAllText(path, json);
        }

        public Test Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Test>(json, options);
        }
    }
}