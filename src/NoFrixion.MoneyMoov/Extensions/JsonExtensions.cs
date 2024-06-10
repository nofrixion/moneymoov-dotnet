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
// MIT.
//-----------------------------------------------------------------------------

using System.Text.Json;
using System.Net.Http.Json;
using Ardalis.GuardClauses;

namespace NoFrixion.MoneyMoov;

public static class JsonExtensions
{
    public static string ToJsonFlat<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, MoneyMoovJson.GetSystemTextSerialiserOptions());
    }

    public static string ToJsonFormatted<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, MoneyMoovJson.GetSystemTextSerialiserOptions(true));
    }

    public static T? FromJson<T>(this string json)
    {
        Guard.Against.NullOrWhiteSpace(json, nameof(json));

        return JsonSerializer.Deserialize<T>(json, MoneyMoovJson.GetSystemTextSerialiserOptions());
    }

    public static JsonContent ToJsonContent<T>(this T obj)
    {
        return JsonContent.Create(obj, options: MoneyMoovJson.GetSystemTextSerialiserOptions());
    }
}
