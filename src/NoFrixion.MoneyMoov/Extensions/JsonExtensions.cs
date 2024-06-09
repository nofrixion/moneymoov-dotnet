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

namespace NoFrixion.MoneyMoov;

public static class JsonExtensions
{
    public static readonly JsonSerializerOptions SerialiseOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static string ToJsonFormatted<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, SerialiseOptions);
    }

    public static T? FromJson<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(json, SerialiseOptions);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public static JsonContent ToJsonContent<T>(this T obj)
    {
        return JsonContent.Create(obj, options: SerialiseOptions);
    }
}
