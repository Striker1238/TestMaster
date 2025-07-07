using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestMaster.Models.App;

namespace TestMaster.Services
{
    public class QuestionJsonConverter : JsonConverter<IQuestion>
    {
        public override IQuestion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var type = doc.RootElement.GetProperty("$type").GetString();
            return (IQuestion)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), Type.GetType(type), options);
        }

        public override void Write(Utf8JsonWriter writer, IQuestion value, JsonSerializerOptions options)
        {
            var type = value.GetType().AssemblyQualifiedName;
            using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));

            writer.WriteStartObject();
            writer.WriteString("$type", type);
            foreach (var property in doc.RootElement.EnumerateObject())
                property.WriteTo(writer);
            writer.WriteEndObject();
        }
    }
}