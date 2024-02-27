// -----------------------------------------------------------------------------
//   Filename: PayrunEvent.cs
// 
//   Description: Payrun event model:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   27 2 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class PayrunEvent
{
    public Guid? UserID { get; set; }
    
    public string Notes { get; set; } = null!;

    public PayrunEventTypeEnum Type { get; set; }
    
    public DateTimeOffset Inserted { get; set; }
    
    public User? User { get; set; }
}