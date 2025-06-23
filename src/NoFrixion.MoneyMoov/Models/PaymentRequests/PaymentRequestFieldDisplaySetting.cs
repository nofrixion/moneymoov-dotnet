// -----------------------------------------------------------------------------
//  Filename: PaymentRequestFieldDisplaySetting.cs
// 
//  Description: The model used to define the display settings for fields in a payment request.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  04 Jun 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestFieldDisplaySetting
{
    /// <summary>
    /// The field for which the display settings are defined.
    /// </summary>
    public PaymentRequestPayerVisibleFieldsEnum Field { get; set; }
    
    /// <summary>
    /// If false, the field will not be displayed on the hosted payment page.
    /// </summary>
    public bool DisplayOnHostedPaymentPage { get; set; }
    
    /// <summary>
    /// If false, the field will not be displayed on the payment receipt.
    /// </summary>
    public bool DisplayOnPaymentReceipt { get; set; }
}