//-----------------------------------------------------------------------------
// Filename: JurisdictionEnum.cs
// 
// Description: List of the jurisdictions, for companies and merchants,
// supported by NoFrixion.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 14 Dec 2021  Aaron Clauson   Migrated from compliance project.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum JurisdictionEnum
{
    /// <summary>
    /// Ireland.
    /// </summary>
    IE,

    /// <summary>
    /// United Kingdom.
    /// </summary>
    UK,

    /// <summary>
    /// European Union, other than Ireland.
    /// </summary>
    EU
}
