//-----------------------------------------------------------------------------
// Filename: JsonStringEnumMemberConverter.cs
//
// Description: Converter for deserialsing strings to enums. Allows both the enum
// value and the member attribute to be used as the string value.
//
// Author(s):
// Aaron Clauson
// 
// History:
// 09 Jun 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Json;

public class JsonStringEnumMemberConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(EnumMemberConverter<>).MakeGenericType(typeToConvert);
        var converter = (JsonConverter?)Activator.CreateInstance(converterType);
        if (converter == null)
        {
            throw new InvalidOperationException($"Unable to create converter for {typeToConvert}");
        }
        return converter;
    }

    private class EnumMemberConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var enumText = reader.GetString();

                // Check for EnumMember attribute values
                foreach (var field in typeof(T).GetFields())
                {
                    if (field.GetCustomAttribute<EnumMemberAttribute>() is EnumMemberAttribute attribute)
                    {
                        if (string.Equals(attribute.Value, enumText, StringComparison.OrdinalIgnoreCase))
                        {
                            return (T)(field.GetValue(null) ?? default(T));
                        }
                    }
                    else if (string.Equals(field.Name, enumText, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)(field.GetValue(null) ?? default(T));
                    }
                }

                // If not found, try parsing directly
                if (Enum.TryParse(enumText, true, out T result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intValue))
                {
                    return (T)(object)intValue;
                }
            }

            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to enum \"{typeof(T)}\".");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var field = typeof(T).GetField(value.ToString());
            if (field?.GetCustomAttribute<EnumMemberAttribute>() is EnumMemberAttribute attribute)
            {
                writer.WriteStringValue(attribute.Value);
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
