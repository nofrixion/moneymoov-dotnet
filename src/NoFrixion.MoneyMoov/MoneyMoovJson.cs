//-----------------------------------------------------------------------------
// Filename:  MoneyNoovJsonS.cs
//
// Description: JSON serialiser options for wotking with the MoneyMoov API.
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

using NoFrixion.MoneyMoov.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NoFrixion.MoneyMoov;

public class MoneyMoovJson
{
    public static JsonSerializerOptions GetSystemTextSerialiserOptions(bool writeIndented = false)
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,

            // Set the JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull) attribute on properties to ignore null values.
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 

            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = writeIndented,
            Converters =
            {
                // Allow enum values or member attribute values,e.g. [EnumMember(Value = "xxx")] to be deserialised from strings.
                new JsonStringEnumMemberConverter(),

                // Allows "true" and "false" strings to be deserialised to booleans.
                new BooleanAsStringConverter(),

                // Newtonsoft allows numeric values to be deserialised from strings.
                // This is not the default behaviour in System.Text.Json so use a custom converter.
                new NumericConverter<int>(),
                new NumericConverter<long>(),
                new NumericConverter<float>(),
                new NumericConverter<double>(),
                new NumericConverter<decimal>()
            }
        };
    }
}
