using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarbershopBookApi.Domain.Converter;

public class CustomDateTimeConverter : JsonConverter<List<DateTime>>
{
    private readonly string _format = "yyyy-MM-ddTHH:mm";

    public override List<DateTime>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new List<DateTime>();
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected start of array");
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                return result;
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException("Expected string value");
            var dateString = reader.GetString();
            if (DateTime.TryParseExact(dateString, _format, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out var date))
            {
                result.Add(date);
            }
            else
            {
                throw new JsonException($"Invalid date format. Expected format: {_format}");   
            }
        }
        throw new JsonException("UnExpected end of array");
    }

    public override void Write(Utf8JsonWriter writer, List<DateTime> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var dateTime in value)
        {
            writer.WriteStringValue(dateTime.ToString(_format, CultureInfo.InvariantCulture));
        }
        writer.WriteEndArray();
    }
}