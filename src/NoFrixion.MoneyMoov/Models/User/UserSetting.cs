//-----------------------------------------------------------------------------
// Filename: UserSetting.cs
//
// Description: Represents a user setting.
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 07 Oct 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserSetting
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;

    public string? Description { get; set; }
}