//-----------------------------------------------------------------------------
// Filename: NumericConverter.cs
//
// Description: Converter for deserialsing strings numeric values.
//
// Author(s):
// Aaron Clauson
// 
// History:
// 09 Jun 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov.Json;

public class NumericConverter<T> : JsonConverter<T> where T : struct, IConvertible
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return default; // Handle empty strings as null/zero equivalent
            }
            try
            {
                return (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(stringValue) ?? default(T));
            }
            catch (Exception ex)
            {
                throw new JsonException($"Unable to convert \"{stringValue}\" to {typeof(T)}.", ex);
            }
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)reader.GetInt32();
            if (typeof(T) == typeof(long))
                return (T)(object)reader.GetInt64();
            if (typeof(T) == typeof(float))
                return (T)(object)reader.GetSingle();
            if (typeof(T) == typeof(double))
                return (T)(object)reader.GetDouble();
            if (typeof(T) == typeof(decimal))
                return (T)(object)reader.GetDecimal();
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return default; // Handle null values as default(T)
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing {typeof(T)}.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
