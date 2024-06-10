﻿//-----------------------------------------------------------------------------
// Filename: BooleanStringConverter.cs
//
// Description: Converter for deserialsing strings to booleans.
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

using System.Text.Json.Serialization;
using System.Text.Json;

namespace NoFrixion.MoneyMoov.Json;

public class BooleanAsStringConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (bool.TryParse(stringValue, out bool boolValue))
            {
                return boolValue;
            }
            throw new JsonException($"Unable to convert \"{stringValue}\" to boolean.");
        }
        else if (reader.TokenType == JsonTokenType.True)
        {
            return true;
        }
        else if (reader.TokenType == JsonTokenType.False)
        {
            return false;
        }
        throw new JsonException($"Unexpected token parsing boolean. Expected String, True, or False, got {reader.TokenType}.");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
         writer.WriteBooleanValue(value);
    }
}
