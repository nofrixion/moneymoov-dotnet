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
}