// -----------------------------------------------------------------------------
//  Filename: PayoutDocumentCreate.cs
// 
//  Description: Contains details of a payout document.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  28 11 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Document associated with a payout.
/// </summary>
public class PayoutDocumentCreate
{
    /// <summary>
    /// Type of the document.
    /// </summary>
    [Required]
    public PayoutDocumentTypesEnum DocumentType { get; set; } = PayoutDocumentTypesEnum.Other;
    
    /// <summary>
    /// Used to identify the document.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string? Title { get; set; }
    
    /// <summary>
    /// Additional information about the document. Optional.
    /// </summary>
    [MaxLength(256)]
    public string? Description { get; set; }

    /// <summary>
    /// Currency of the document, if applicable. Optional.
    /// </summary>
    public CurrencyTypeEnum? Currency { get; set; }

    /// <summary>
    /// Amount of the document, if applicable. Optional.
    /// </summary>
    public decimal? Amount { get; set; }
    
    /// <summary>
    /// Allows to associate an external ID to the document. Optional.
    /// </summary>
    [MaxLength(128)]
    public string? ExternalID { get; set; }
}