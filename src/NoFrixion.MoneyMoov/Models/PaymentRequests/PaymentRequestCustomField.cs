//-----------------------------------------------------------------------------
// Filename: PaymentRequestCustomField.cs
//
// Description: The model used for a custom field for a payment request.
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

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestCustomField
{
    public required string Name { get; set; }
    
    public required string Value { get; set; }
    
    public bool DisplayForPayer { get; set; }
    
    public int DisplayOrder { get; set; }
}