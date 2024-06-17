//-----------------------------------------------------------------------------
// Filename: JsonExtensions.cs
//
// Description: Extension methods for the common JSON operations with option
// to use either System.Text.Json or Newtonsoft.Json.
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

using System.Net.Http.Json;
using Ardalis.GuardClauses;
using Newtonsoft.Json;

namespace NoFrixion.MoneyMoov;

public static class JsonExtensions
{
    public static string ToJsonFlat<T>(this T obj, bool usePascalCase = false)
        => obj.ToJsonFlatNewtonsoft(usePascalCase);

    public static string ToJsonFormatted<T>(this T obj, bool usePascalCase = false)
        => obj.ToJsonFormattedNewtonsoft(usePascalCase);

    public static T? FromJson<T>(this string json, bool usePascalCase = false)
        => json.FromJsonNewtonsoft<T>(usePascalCase);

    public static JsonContent ToJsonContent<T>(this T obj)
    {
        return JsonContent.Create(obj, options: MoneyMoovJson.GetSystemTextSerialiserOptions());
    }

    private static string ToJsonFlatSystemText<T>(this T obj, bool usePascalCase = false)
        => System.Text.Json.JsonSerializer.Serialize(obj, MoneyMoovJson.GetSystemTextSerialiserOptions());

    private static string ToJsonFormattedSystemText<T>(this T obj, bool usePascalCase)
        => System.Text.Json.JsonSerializer.Serialize(obj, MoneyMoovJson.GetSystemTextSerialiserOptions(true, usePascalCase: usePascalCase));

    private static T? FromJsonSystemText<T>(this string json, bool usePascalCase)
    {
        Guard.Against.NullOrWhiteSpace(json, nameof(json));

        return System.Text.Json.JsonSerializer.Deserialize<T>(json, MoneyMoovJson.GetSystemTextSerialiserOptions(usePascalCase: usePascalCase));
    }

    private static string ToJsonFlatNewtonsoft<T>(this T obj, bool usePascalCase)
        => JsonConvert.SerializeObject(obj, MoneyMoovJson.GetNewtonsoftSerialiserSettings(usePascalCase: usePascalCase));

    private static string ToJsonFormattedNewtonsoft<T>(this T obj, bool usePascalCase)
        => JsonConvert.SerializeObject(obj, MoneyMoovJson.GetNewtonsoftSerialiserSettings(true, usePascalCase: usePascalCase));

    private static T? FromJsonNewtonsoft<T>(this string json, bool usePascalCase)
    {
        Guard.Against.NullOrWhiteSpace(json, nameof(json));

        return JsonConvert.DeserializeObject<T>(json, MoneyMoovJson.GetNewtonsoftSerialiserSettings(usePascalCase: usePascalCase));
    }
}
