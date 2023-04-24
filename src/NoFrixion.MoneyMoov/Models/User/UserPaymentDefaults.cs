// -----------------------------------------------------------------------------
//  Filename: UserPaymentDefaults.cs
// 
//  Description: User default settings:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 04 2023  Donal O'Connor   Created, Harcourt St., Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public record UserPaymentDefaults
{
    public Guid UserID { get; set; }
    
    public bool Pisp { get; set; }

    public bool PispPriorityBank { get; set; }
    
    public Guid? PispPriorityBankID { get; set; } 
    
    public bool Card { get; set; }

    public bool CardAuthorizeOnly { get; set; }

    public bool ApplePay { get; set; }

    public bool Lightning { get; set; }
}