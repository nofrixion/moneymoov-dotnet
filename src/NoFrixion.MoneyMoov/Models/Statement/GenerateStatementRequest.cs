// -----------------------------------------------------------------------------
//  Filename: GenerateStatementRequest.cs
// 
//  Description: Model for generating a transaction statement.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  03 05 2024  Axel Granillo   Created, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Model for generating a transaction statement.
/// </summary>
public class GenerateStatementRequest
{
    /// <summary>
    /// ID of the account.
    /// </summary>
    public Guid AccountID { get; set; }

    /// <summary>
    /// Minimum transaction date for the statement.
    /// </summary>
    public DateTimeOffset FromDate { get; set; }

    /// <summary>
    /// Maximum transaction date for the statement.
    /// </summary>
    public DateTimeOffset ToDate { get; set; }
    
    /// <summary>
    /// File format to save the statement as. Defaults to PDF.
    /// </summary>
    public TransactionStatementFormat Format { get; set; } = TransactionStatementFormat.Pdf;
}