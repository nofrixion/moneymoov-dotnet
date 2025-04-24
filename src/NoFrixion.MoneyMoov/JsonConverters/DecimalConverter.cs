//-----------------------------------------------------------------------------
// Filename: DecimalConverter.cs
//
// Description: JSON serialisation converter to write decimals with at least two
// decimal places. This assists consumers of the API to recognise decimal amounts
// represent dollar and cent equivalents.
//
// Author(s):
// Aaron Clauson
// 
// History:
// 24 Apr 2025  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System.Globalization;

namespace NoFrixion.MoneyMoov.Json;

public class DecimalConverter : JsonConverter<decimal>
{
    public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
    {
        if (value != Math.Round(value, 2))
        {
            writer.WriteValue(value);
        }
        else
        {
            writer.WriteRawValue(value.ToString("0.00", CultureInfo.InvariantCulture));
        }
    }

    public override decimal ReadJson(JsonReader reader,
                                     Type objectType,
                                     decimal existingValue,
                                     bool hasExistingValue,
                                     JsonSerializer serializer)
    {
        // Use default decimal parsing.
        return Convert.ToDecimal(reader.Value, CultureInfo.InvariantCulture);
    }
}
