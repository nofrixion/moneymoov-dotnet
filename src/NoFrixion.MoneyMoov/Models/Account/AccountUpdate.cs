// -----------------------------------------------------------------------------
//  Filename: AccountUpdate.cs
// 
//  Description: Contains details of a account update request:
// 
//  Author(s):
//  Pablo Maldonado (pablo@nofrixion.com)
// 
//  History:
//  18 09 2023  Pablo Maldonado   Created, Punta Colorada, Maldonado, Uruguay.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class AccountUpdate
{
    public Guid? AccountID { get; set; }

    public string? AccountName { get; set; }
}