﻿//-----------------------------------------------------------------------------
// Filename: Tag.cs
//
// Description: Represents a structure for attaching a descriptive tag to an
// entity, such as a payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 14 Mar 2023  Aaron Clauson       Created, Harcourt St, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class Tag
{
    public Guid ID { get; set; }
    
    [Required]
    public Guid MerchantID { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    public string ColourHex { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
