//-----------------------------------------------------------------------------
// Filename: PaymentRequestPayerVisibleFieldsEnum.cs
//
// Description: Properties of a payment request that are visible to the payer on hosted payment page, receipts etc.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 04 June 2025  Saurav Maiti  Created, Hamilton gardens, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum PaymentRequestPayerVisibleFieldsEnum
{
    None,
    Description,
    Customer,
    DueDate,
}