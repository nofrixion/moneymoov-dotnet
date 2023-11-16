//  -----------------------------------------------------------------------------
//   Filename: PayoutActivity.cs
// 
//   Description: Payout activity:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   16 11 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public enum ActivityAction
{
    Created = 0,
    Updated = 1,
    Queued = 2,
    Initiated = 3,
    Settled = 4,
}

public class PayoutActivity
{
    public Guid UserID { get; set; }

    public string? UserName { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public PayoutStatus Status { get; set; }
    
    public ActivityAction ActivityAction { get; set; }
        
}