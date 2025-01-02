// -----------------------------------------------------------------------------
//  Filename: MerchantAuthorisationSetting.cs
// 
//  Description: MerchantAuthorisationSettings model.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  02 01 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Roles;

namespace NoFrixion.MoneyMoov.Models.AuthorisationSettings;

public class MerchantAuthorisationSetting
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public decimal? AmountLower { get; set; }

    public decimal? AmountUpper { get; set; }

    public bool BeneficiariesOnly { get; set; }

    public int NumberOfAuthorisers { get; set; }

    public bool CreatorCantAuthorise { get; set; }

    public bool EditorCantAuthorise { get; set; }
    
    public AuthorisationType AuthorisationType { get; set; }
    
    public List<Role> Roles { get; set; } = new();
    
    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}