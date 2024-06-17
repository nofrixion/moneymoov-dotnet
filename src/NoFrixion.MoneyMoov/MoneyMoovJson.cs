//-----------------------------------------------------------------------------
// Filename:  MoneyNoovJson.cs
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

using Newtonsoft.Json;
using NoFrixion.MoneyMoov.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace NoFrixion.MoneyMoov;

public class MoneyMoovJson
{
    public static JsonSerializerOptions GetSystemTextSerialiserOptions(bool writeIndented = false, bool usePascalCase = false)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
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

        if(usePascalCase)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        return options;
    }

    public static JsonSerializerSettings GetNewtonsoftSerialiserSettings(bool writeIndented = false, bool usePascalCase = false)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = writeIndented ? Formatting.Indented : Formatting.None,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Converters =
            {
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };

        if(usePascalCase == false)
        {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        return settings;
    }
}
