// -----------------------------------------------------------------------------
//   Filename: PayrunAuthorisation.cs
// 
//   Description: Represents a payrun authorisation request:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   13 2 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunAuthorisation
{
    public Guid ID { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTimeOffset? ScheduledDate { get; set; }
}