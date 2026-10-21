// -----------------------------------------------------------------------------
//  Filename: MerchantTokenPermissionsEnumExtensions.cs
// 
//  Description: Contains extension methods for the MerchantTokenPermissionsEnum enum.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  11 09 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public static class MerchantTokenPermissionsEnumExtensions
{
    /// <summary>
    /// Permissions that are restricted and require an IP address whitelist.
    /// </summary>
    public static bool IsPrivilegedPermission(this MerchantTokenPermissionsEnum permission)
    {
        if (permission.HasFlag(MerchantTokenPermissionsEnum.ViewPaymentAccount))
        {
            return true;
        }
        if (permission.HasFlag(MerchantTokenPermissionsEnum.ViewPayout))
        {
            return true;
        }
        if (permission.HasFlag(MerchantTokenPermissionsEnum.ViewTransactions))
        {
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Returns true if the permission is a backend-only permission that cannot be
    /// configured by users via the API. These permissions can only be set directly
    /// in the database by NoFrixion staff.
    /// </summary>
    public static bool IsBackendOnlyPermission(this MerchantTokenPermissionsEnum permission)
    {
        return permission.HasFlag(MerchantTokenPermissionsEnum.CanExecuteVoP);
    }
    
    /// <summary>
    /// Returns true if any of the permissions in the set are backend-only permissions
    /// that cannot be configured by users.
    /// </summary>
    public static bool ContainsBackendOnlyPermission(this IEnumerable<MerchantTokenPermissionsEnum> permissions)
    {
        return permissions.Any(p => p.IsBackendOnlyPermission());
    }
}