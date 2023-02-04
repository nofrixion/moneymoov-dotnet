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

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NoFrixion.MoneyMoov;

public enum JurisdictionEnum
{
    /// <summary>
    /// Ireland.
    /// </summary>
    [Display(Name = "Ireland")]
    IE,

    /// <summary>
    /// United Kingdom.
    /// </summary>
    [Display(Name = "United Kingdom")]
    UK,

    /// <summary>
    /// European Union, other than Ireland.
    /// </summary>
    [Display(Name = "European Union")]
    EU
}
