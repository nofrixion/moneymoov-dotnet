// -----------------------------------------------------------------------------
//  Filename: MerchantAuthorisationRoleSetting.cs
// 
//  Description:  Merchant Authorisation Role Setting:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  20 03 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.AuthorisationSettings;

public class MerchantAuthorisationRoleSetting
{
    public Guid RoleID { get; set; }
    
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid MerchantID { get; set; }
    
    public int? MinNumberAuthorisers { get; set; }

    public int? MaxNumberAuthorisers { get; set; }
    
    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}