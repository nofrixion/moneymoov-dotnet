//  -----------------------------------------------------------------------------
//   Filename: PayoutEvent.cs
// 
//   Description: Payout event:
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

public class PayoutEvent
{
    public Guid UserID { get; set; }

    public string? UserName { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public PayoutStatus Status { get; set; }
    
    public PayoutEventTypesEnum EventType { get; set; }
}