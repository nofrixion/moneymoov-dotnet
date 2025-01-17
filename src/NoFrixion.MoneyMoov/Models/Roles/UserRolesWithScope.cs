//-----------------------------------------------------------------------------
// Filename: UserRoleWithScope.cs
// 
// Description: Represents a user role on a merchant
// which includes the scope(number of accounts)
// 
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 16 Jan 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland. 
// 
// License:
// MIT
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles;

public class UserRoleWithScope
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    public Guid UserID { get; set; }
    
    /// <summary>
    /// The ID of the role the user has.
    /// </summary>
    public Guid RoleID { get; set; }
    
    /// <summary>
    /// The ID of the merchant the role is in.
    /// </summary>
    public Guid MerchantID { get; set; }
    
    /// <summary>
    /// The name of the role the user has.
    /// </summary>
    public string RoleName { get; set; } = null!;
    
    /// <summary>
    /// The number of accounts the user has access to in the merchant for the role.
    /// </summary>
    public int AccountsCount { get; set; }
}