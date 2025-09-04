// -----------------------------------------------------------------------------
//  Filename: AuthorisationType.cs
// 
//  Description: Authorisation type enum:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  07 11 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum AuthorisationType
{
    None = 0,
    Payout = 1,
    Rule = 2,
    Beneficiary = 3,
    Payrun = 4,
    MerchantToken = 5,
    UserInvite = 6,
    RoleUser = 7
}