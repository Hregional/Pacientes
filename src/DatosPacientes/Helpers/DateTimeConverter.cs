namespace DatosPacientes.Helpers
{
    // Helpers/DateTimeConverter.cs
    using System.Text.Json;
    using System.Text.Json.Serialization;

    namespace DatosPacientes.Helpers
    {
        public class DateTimeConverter : JsonConverter<DateTime?>
        {
            private readonly string _format;

            public DateTimeConverter(string format = "dd-MM-yyyy")
            {
                _format = format;
            }

            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return null;

                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    return date;

                return null;
            }

            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    writer.WriteStringValue(value.Value.ToString(_format));
                else
                    writer.WriteNullValue();
            }
        }
    }
}
