// -----------------------------------------------------------------------------
//  Filename: PaymentRequestMinimal.cs
// 
//  Description: Contains a minimal number of fields for a payment request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  25 11 2022  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestMinimal
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public string? MerchantName { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency of the request.
    /// </summary>
    [EnumDataType(typeof(CurrencyTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;
    
    /// <summary>
    /// An optional description for the payment request. If set this field will appear
    /// on the transaction record for some card processors.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The card processor
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentProcessorsEnum PaymentProcessor { get; set; }

    public string? CallbackUrl { get; set; }

    public string? CardStripePaymentIntentSecret { get; set; }

    /// <summary>
    /// The jwk containing the public key
    /// </summary>
    public string? Jwk { get; set; }
}