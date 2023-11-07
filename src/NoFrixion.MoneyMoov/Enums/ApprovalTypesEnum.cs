// -----------------------------------------------------------------------------
//  Filename: ApproveTypesEnum.cs
// 
//  Description: List of the types of approvals that the identity server can handle.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  14 02 2023  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum ApproveTypesEnum
{
    None = 0,

    Payout = 1,

    Rule = 2,

    Message = 3,

    UserToken = 4,

    BatchPayout = 5,

    PayByNoFrixion = 6,
    
    Beneficiary = 7
}