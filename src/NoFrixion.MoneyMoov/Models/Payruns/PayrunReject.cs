// -----------------------------------------------------------------------------
//   Filename: PayrunReject.cs
// 
//   Description: Represents the model to reject a payrun:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   21 2 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunReject
{
    public Guid ID { get; set; }
    
    public string? Reason { get; set; }
}