//  -----------------------------------------------------------------------------
//   Filename: PayrunStatus.cs
// 
//   Description: Payrun status enum:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   04 12 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum PayrunStatus
{
    None = 0,
    Draft = 1,
    Queued = 2,
    Submitted = 3,
    Completed = 5, 
    Rejected = 6,
    AuthorisationPending = 7,
    PayoutsCreated = 8,
    Edited = 9,
}