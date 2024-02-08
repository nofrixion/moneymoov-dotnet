//-----------------------------------------------------------------------------
// Filename: PayrunCreate.cs
// 
// Description: Represent the model to create a new Payrun from a list of
// invoices.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 04 Dec 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunCreate
{
   public string Name { get; set; } = string.Empty;

    public List<PayrunInvoice> Invoices { get; set; } = new List<PayrunInvoice>();
}