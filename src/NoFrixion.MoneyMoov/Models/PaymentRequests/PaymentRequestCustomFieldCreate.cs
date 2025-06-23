//-----------------------------------------------------------------------------
// Filename: PaymentRequestCustomFieldCreate.cs
//
// Description: The model used for creating a custom field for a payment request.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 30 Apr 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestCustomFieldCreate
{
    /// <summary>
    /// The name of the custom field.
    /// </summary>
    [Required(ErrorMessage = "Custom field name is required", AllowEmptyStrings = false)]
    public string? Name { get; set; }
    
    /// <summary>
    /// The value of the custom field.
    /// </summary>
    [Required(ErrorMessage = "Custom field value is required", AllowEmptyStrings = false)]
    public string? Value { get; set; }
    
    /// <summary>
    /// If true, the custom field will be displayed on the hosted payment page.
    /// </summary>
    public bool DisplayOnHostedPaymentPage { get; set; }
    
    /// <summary>
    /// If true, the custom field will be displayed on the payment receipt.
    /// </summary>
    public bool DisplayOnPaymentReceipt { get; set; }
    
    /// <summary>
    /// The display order of the custom field. The lowest number is displayed first.
    /// This is used to determine the order in which the custom fields are displayed
    /// on the UI, for example, on the hosted payment page and payment receipt.
    /// </summary>
    public int DisplayOrder { get; set; }
}