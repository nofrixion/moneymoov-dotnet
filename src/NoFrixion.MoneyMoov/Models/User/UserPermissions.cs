// -----------------------------------------------------------------------------
//  Filename: UserPermissions.cs
// 
//  Description: The business layer representation of a user's permissions.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  14 02 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserPermissions
{
    public List<MerchantPermission> MerchantPermissions { get; set; } = new();
}

public class MerchantPermission
{
    public required string MerchantID { get; set; }

    public List<string> Permissions { get; set; } = new();

    public Dictionary<string, List<string>> AccountPermissions { get; set; } = new();
}