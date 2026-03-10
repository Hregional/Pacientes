using System.Text.Json;
using System.Text.Json.Serialization;

namespace DatosPacientes.Helpers.DatosPacientes.Helpers
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public DateTimeConverter(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                if (DateTime.TryParseExact(dateString, _format, null, System.Globalization.DateTimeStyles.None, out var date))
                {
                    return date;
                }
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
