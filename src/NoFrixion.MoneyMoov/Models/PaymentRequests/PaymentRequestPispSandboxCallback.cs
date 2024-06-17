//-----------------------------------------------------------------------------
// Filename: PaymentRequestPispSandboxCallback.cs
// 
// Description: Payment initiation callback model for pay by bank simulations.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 16 Jun 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestPispSandboxCallback
{
   public decimal Amount { get; set; }

   public string? Institution { get; set; }

   public string? PaymentInitiationID { get; set; }
            
   public string? ErrorDescription { get; set; }
            
   public bool DoSimulateSettlementFailure { get; set; }
}
