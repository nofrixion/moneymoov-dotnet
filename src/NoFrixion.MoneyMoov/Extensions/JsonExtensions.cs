//-----------------------------------------------------------------------------
// Filename: JsonExtensions.cs
//
// Description: Extension methods for the common JSON operations.
//
// Author(s):
// Aaron Clauson
// 
// History:
// 08 Jun 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using Ardalis.GuardClauses;
using NoFrixion.MoneyMoov.Json;

namespace NoFrixion.MoneyMoov;

public static class JsonExtensions
{
    public static JsonSerializerOptions GetSerialiserOptions(bool writeIndented = false)
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = writeIndented,
            Converters =
            {
                // Allow enum values or member attribute values,e.g. [EnumMember(Value = "xxx")] to be deserialised from strings.
                new JsonStringEnumMemberConverter(),

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

    public static string ToJsonFlat<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, GetSerialiserOptions());
    }

    public static string ToJsonFormatted<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, GetSerialiserOptions(true));
    }

    public static T? FromJson<T>(this string json)
    {
        Guard.Against.NullOrWhiteSpace(json, nameof(json));

        return JsonSerializer.Deserialize<T>(json, GetSerialiserOptions());
    }

    public static JsonContent ToJsonContent<T>(this T obj)
    {
        return JsonContent.Create(obj, options: GetSerialiserOptions());
    }
}
