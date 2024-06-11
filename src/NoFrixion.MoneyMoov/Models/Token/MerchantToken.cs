// -----------------------------------------------------------------------------
//  Filename: MerchantToken.cs
// 
//  Description: Biz friendly representation of the MerchantToken database model.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  17 01 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class MerchantToken
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public string Description { get; set; }

    public MerchantTokenPermissionsEnum Permissions { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// The JWT merchant token. It will only be available when the merchant token is
    /// initially created. The token is not stored by NoFrixion.
    /// </summary>
    public string Token { get; set; }
}