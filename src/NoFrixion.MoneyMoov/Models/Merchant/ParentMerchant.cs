// -----------------------------------------------------------------------------
//  Filename: ParentMerchant.cs
// 
//  Description: Model to represent a Parent Merchant in MoneyMoov.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  09 10 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class ParentMerchant
{
    public Guid ID { get; set; }
    
    public string? Name { get; set; }
}