// -----------------------------------------------------------------------------
//  Filename: PayoutDocumentTypesEnum.cs
// 
//  Description: Enum for the different types of payout documents.
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

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Enum for the different types of payout documents.
/// </summary>
public enum PayoutDocumentTypesEnum
{
    /// <summary>
    /// Default general value.
    /// </summary>
    Other,
    
    /// <summary>
    /// Invoice.
    /// </summary>
    Invoice,
    
    /// <summary>
    /// Purchase order.
    /// </summary>
    PurchaseOrder,
}