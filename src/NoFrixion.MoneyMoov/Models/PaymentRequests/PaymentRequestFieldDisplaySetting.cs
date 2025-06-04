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
    public PaymentRequestPayerVisibleFieldsEnum Field { get; set; }
    
    public bool DisplayForPayer { get; set; }
}