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
    public Guid ID { get; set; }

    public Guid UserID { get; set; }

    public Guid MerchantID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public UserRolesEnum RoleType { get; set; }
}

