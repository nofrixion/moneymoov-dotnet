//-----------------------------------------------------------------------------
// Filename: FxRate.cs
// 
// Description: A model representing a Foreign Exchange (FX) rate.
// 
// Author(s):
// Aaron Clasuon (aaron@nofrixion.com)
// 
// History:
// 17 Jul 2025  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Represents a Foreign Exchange (FX) rate.
/// </summary>
public class FxRate
{
    public CurrencyTypeEnum SourceCurrency { get; set; }

    public CurrencyTypeEnum DestinationCurrency { get; set; }

    /// <summary>
    /// The price at which the transaction will buy the source currency 
    /// using the destination currency.
    /// </summary>
    public decimal ExchangeRate { get; set; }

    public string? QuoteID { get; set; }

    public DateTimeOffset ExpiryTime { get; set; }
}