// -----------------------------------------------------------------------------
//  Filename: GuidExtensions.cs
// 
//  Description: Contains extension methods for Guid types:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  14 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Extensions;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return guid.GetValueOrDefault() == Guid.Empty;
    }
}