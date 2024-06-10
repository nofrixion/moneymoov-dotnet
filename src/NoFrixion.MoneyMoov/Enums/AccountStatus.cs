// -----------------------------------------------------------------------------
//  Filename: AccountStatus.cs
// 
//  Description: Enum for the PaymentAccount class:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum AccountStatus
{
    ACTIVE = 1,

    INACTIVE = 2,

    BLOCKED = 3,

    CLOSED = 4,

    CLIENTBLOCKED = 5
}