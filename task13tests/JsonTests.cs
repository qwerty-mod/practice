using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using task13;

namespace task13tests
{
    public class JsonTests
    {
        [Fact]
        public void serializeDeserialize_Correct()
        {
            var student = new Student
            {
                FirstName = "Тест",
                LastName = "Пользователь",
                BirthDate = new DateTime(2000, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Математика", Grade = 5 }
                }
            };

            var json = StudentJsonService.SerializeToJson(student);
            var result = StudentJsonService.DeserializeFromJson(json);

            Assert.Equal(student.FirstName, result.FirstName);
            Assert.Equal(student.LastName, result.LastName);
            Assert.Equal(student.BirthDate, result.BirthDate);
            Assert.Single(result.Grades);
        }

        [Fact]
        public void saveAndLoad_Correct()
        {
            var student = new Student
            {
                FirstName = "Файл",
                LastName = "Тест",
                BirthDate = new DateTime(1999, 5, 10),
                Grades = new List<Subject>()
            };

            var path = Path.GetTempFileName();

            try
            {
                StudentJsonService.SaveToFile(student, path);
                var loaded = StudentJsonService.LoadFromFile(path);
                Assert.Equal(student.FirstName, loaded.FirstName);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Fact]
        public void deserialize_invalidBirthDate()
        {
            var json = @"{
                ""FirstName"": ""Неверно"",
                ""LastName"": ""Дата"",
                ""BirthDate"": ""2999-01-01"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentJsonService.DeserializeFromJson(json));
        }

        [Fact]
        public void deserialize_MissingFirstName()
        {
            var json = @"{
                ""FirstName"": """",
                ""LastName"": ""Уик"",
                ""BirthDate"": ""2000-01-01"",
                ""Grades"": []
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentJsonService.DeserializeFromJson(json));
        }

        [Fact]
        public void deserialize_invalidGrade()
        {
            var json = @"{
                ""FirstName"": ""Джон"",
                ""LastName"": ""Уик"",
                ""BirthDate"": ""2000-01-01"",
                ""Grades"": [{ ""Name"": ""Математика"", ""Grade"": 7 }]
            }";

            Assert.Throws<InvalidOperationException>(() =>
                StudentJsonService.DeserializeFromJson(json));
        }
    }
}
