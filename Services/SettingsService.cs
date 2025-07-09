using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TestMaster.Services
{
    public static class SettingsService
    {
        private static JsonDocument _jsonDoc;
        private static string filePath = "appsettings.json";

        static SettingsService()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл настроек '{filePath}' не найден.");

            var json = File.ReadAllText(filePath);
            _jsonDoc = JsonDocument.Parse(json);
        }

        public static string? GetString(string path)
        {
            return TryGetElement(path, out var element) && element.ValueKind == JsonValueKind.String
                ? element.GetString()
                : null;
        }

        public static int? GetInt(string path)
        {
            return TryGetElement(path, out var element) && element.ValueKind == JsonValueKind.Number
                ? element.GetInt32()
                : null;
        }

        public static bool? GetBool(string path)
        {
            return TryGetElement(path, out var element) && element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False
                ? element.GetBoolean()
                : null;
        }

        private static bool TryGetElement(string path, out JsonElement element)
        {
            string[] parts = path.Split(':');
            JsonElement current = _jsonDoc.RootElement;

            foreach (var part in parts)
            {
                if (current.TryGetProperty(part, out var next))
                {
                    current = next;
                }
                else
                {
                    element = default;
                    return false;
                }
            }

            element = current;
            return true;
        }
    }
}
