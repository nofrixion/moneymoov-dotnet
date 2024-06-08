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

namespace NoFrixion.MoneyMoov;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _serialiseOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static string ToJsonFormatted<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, _serialiseOptions);
    }
}
