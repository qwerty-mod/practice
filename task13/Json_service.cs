using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class StudentJsonService
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        static StudentJsonService()
        {
            _options.Converters.Add(new DateOnlyConverter());
        }

        public static string SerializeToJson(Student student)
        {
            return JsonSerializer.Serialize(student, _options);
        }

        public static Student DeserializeFromJson(string json)
        {
            var student = JsonSerializer.Deserialize<Student>(json, _options)
                          ?? throw new InvalidOperationException("десериализация вернула ноль");

            student.Validate(); 

            return student;
        }

        public static void SaveToFile(Student student, string filePath)
        {
            File.WriteAllText(filePath, SerializeToJson(student));
        }

        public static Student LoadFromFile(string filePath)
        {
            return DeserializeFromJson(File.ReadAllText(filePath));
        }
    }
}
