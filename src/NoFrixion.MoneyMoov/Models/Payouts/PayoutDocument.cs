// -----------------------------------------------------------------------------
//  Filename: PayoutDocument.cs
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

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Used for returning a payout document.
/// </summary>
public class PayoutDocument : PayoutDocumentCreate
{
    /// <summary>
    /// Internal ID of the document.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// Date of insertion.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }
}