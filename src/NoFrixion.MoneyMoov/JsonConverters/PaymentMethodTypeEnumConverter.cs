using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using NoFrixion.MoneyMoov.Enums;
using System.Linq;

namespace NoFrixion.MoneyMoov.Json;

public class PaymentMethodTypeEnumConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if(value == null)
        {
            writer.WriteNull();
            return;
        }

        var paymentMethodTypes = (PaymentMethodTypeEnum)value;
        var enumValues = Enum.GetValues(typeof(PaymentMethodTypeEnum))
            .Cast<PaymentMethodTypeEnum>()
            .Where(x => (paymentMethodTypes & x) == x)
            .Select(x => x.ToString())
            .ToList();

        writer.WriteStartArray();
        foreach (var enumValue in enumValues)
        {
            writer.WriteValue(enumValue);
        }
        writer.WriteEndArray();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return PaymentMethodTypeEnum.None;
        }

        var paymentMethodTypes = PaymentMethodTypeEnum.None; // Assuming 'None' is a default value
        var enumValues = new List<string>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray)
                break;

            var readerValue = reader.Value?.ToString();

            if (readerValue  != null)
            {
                enumValues.Add(readerValue);
            }
        }

        foreach (var enumValue in enumValues)
        {
            if (Enum.TryParse(typeof(PaymentMethodTypeEnum), enumValue, out var parsedValue))
            {
                paymentMethodTypes |= (PaymentMethodTypeEnum)parsedValue;
            }
        }

        return paymentMethodTypes;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(PaymentMethodTypeEnum);
    }
}
