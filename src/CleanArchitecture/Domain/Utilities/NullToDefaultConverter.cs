using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Domain.Utilities;

public class NullToDefaultConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null
            ? typeToConvert.IsValueType && Nullable.GetUnderlyingType(typeToConvert) == null
                ? Activator.CreateInstance(typeToConvert)!
                : null!
            : JsonSerializer.Deserialize(ref reader, typeToConvert, options)!;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
