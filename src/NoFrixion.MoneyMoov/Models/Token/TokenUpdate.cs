// -----------------------------------------------------------------------------
//  Filename: TokenUpdate.cs
// 
//  Description: Contains details of a merchant token update request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  01 09 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Attributes;

namespace NoFrixion.MoneyMoov.Models;

public class TokenUpdate
{
    public Guid MerchantID { get; set; }

    public string? Description { get; set; }
    
    /// <summary>
    /// The permissions that the merchant token supports.
    /// </summary>
    public List<MerchantTokenPermissionsEnum>? PermissionTypes { get; set; }
    
    /// <summary>
    /// Optional. If set represents a comma separated list of IP addresses that this token is authorised to be used from.
    /// Attempts to use the token from an IP address not in the list will be rejected.
    /// </summary>
    [IPAddress]
    public string? IPAddressWhitelist { get; set; }
}