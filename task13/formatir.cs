using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (!DateTime.TryParseExact(value, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                throw new JsonException($"неверный формат даты: {value}. ожидаемый формат: {Format}");
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
