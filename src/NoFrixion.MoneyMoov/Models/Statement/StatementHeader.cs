// -----------------------------------------------------------------------------
//  Filename: StatementHeader.cs
// 
//  Description: Class containing header information for a printed transaction statement.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  25 01 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

#nullable disable

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Class containing header information for a printed transaction statement.
/// </summary>
public class StatementHeader
{
    /// <summary>
    /// Name of the merchant's account.
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// Currency of the merchant's account.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; }
    
    /// <summary>
    /// Merchant's account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// IBAN number of the merchant's account.
    /// </summary>
    public string Iban { get; set; }
    
    /// <summary>
    /// Sort code of the merchant's account.
    /// </summary>
    public string SortCode { get; set; }
    
    /// <summary>
    /// NoFrixion's name and address.
    /// </summary>
    public string NoFrixionDetails { get; set; }
    
    /// <summary>
    /// Merchant's name and address.
    /// </summary>
    public string MerchantDetails { get; set; }

    /// <summary>
    /// The statement's period specified in d MMM - d MMM yyyy format.
    /// </summary>
    public string SpecifiedPeriodText { get; set; }
    
    /// <summary>
    /// Account balance from the previous period.
    /// </summary>
    public decimal PreviousBalance { get; set; }
    
    /// <summary>
    /// Total paid-out amount.
    /// </summary>
    public decimal PaidOut { get; set; }
    
    /// <summary>
    /// Total paid-in amount.
    /// </summary>
    public decimal PaidIn { get; set; }
    
    /// <summary>
    /// Current account balance.
    /// </summary>
    public decimal Balance { get; set; }
    
    /// <summary>
    /// Account transactions for the specified date range.
    /// </summary>
    public IEnumerable<Transaction> Transactions { get; set; }
    
    public bool IsTrustAccount { get; set; }
}