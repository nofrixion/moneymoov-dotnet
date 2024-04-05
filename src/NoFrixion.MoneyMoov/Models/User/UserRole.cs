//-----------------------------------------------------------------------------
// Filename: Merchant.cs
// 
// Description: UserRole model
//  Author(s):
// Saurav Maiti      (saurav@nofrixion.com)
// 
// History:
// 19 July 2022     Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
// -----------------------------------------------------------------------------

#nullable disable

namespace NoFrixion.MoneyMoov.Models;

public class UserRole
{
    private static readonly UserRole _empty = new UserRole();
    public static UserRole Empty => _empty;
    public bool IsEmpty => this == _empty;

    public Guid ID { get; set; }

    public Guid UserID { get; set; }

    public Guid MerchantID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public UserRolesEnum RoleType { get; set; }
    
    public DateTimeOffset DateJoined { get; set; }
    
    public string InvitedBy { get; set; }
}

