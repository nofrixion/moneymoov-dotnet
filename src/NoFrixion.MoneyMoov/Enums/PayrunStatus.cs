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
    Submitted = 2,
    Completed = 3, 
    Rejected = 4,
}