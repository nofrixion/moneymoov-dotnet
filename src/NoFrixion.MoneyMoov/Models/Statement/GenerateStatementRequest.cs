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
    /// Month number for the start of the range.
    /// </summary>
    public short FromMonth { get; set; }
    
    /// <summary>
    /// Year for the start of the range.
    /// </summary>
    public short FromYear { get; set; }
    
    /// <summary>
    /// Month number for the limit of the range.
    /// </summary>
    public short ToMonth { get; set; }
    
    /// <summary>
    /// Year for the limit of the range.
    /// </summary>
    public short ToYear { get; set; }
    
    /// <summary>
    /// File format to save the statement as. Defaults to PDF.
    /// </summary>
    public TransactionStatementFormat Format { get; set; } = TransactionStatementFormat.Pdf;
}