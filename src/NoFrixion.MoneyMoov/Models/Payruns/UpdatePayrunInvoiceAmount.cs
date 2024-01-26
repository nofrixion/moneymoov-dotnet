// -----------------------------------------------------------------------------
// Filename: UpdatePayrunInvoice.cs
// 
// Description: Contains details to update the invoice's amount to pay request
//
// Author(s):
// Pablo Maldonado (pablo@nofrixion.com)
// 
// History:
//  26 Jan 2024  Pablo Maldonado   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UpdatePayrunInvoiceAmount
{
    public decimal AmountToPay { get; set; }
}