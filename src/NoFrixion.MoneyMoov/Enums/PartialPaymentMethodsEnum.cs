//-----------------------------------------------------------------------------
// Filename: PartialPaymentMethodsEnum.cs
//
// Description: An enum containing the different options for dealing with
// partial payments, or not, payments for a payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 10 Nov 2022  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PartialPaymentMethodsEnum
{
    /// <summary>
    /// The default option is to not support partial payments and instead
    /// require a single payment in full.
    /// </summary>
    None = 0,

    /// <summary>
    /// Accept multiple partial payments.
    /// </summary>
    Partial = 1
}