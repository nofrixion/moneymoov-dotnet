// -----------------------------------------------------------------------------
// Filename: CurrencyTypeEnum.cs
// 
// Description: List of the currencies supported by NoFrixion.
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 26 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 03 Dec 2021  Aaron Clauson   Renamed from Currency to CurrencyTypeEnum.
// 13 Feb 2025  Aaron Clauson   Added USD.
// 23 Jun 2026  Axel Granillo   Added support for additional currencies and marked BGN and ISK as obsolete.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov;

public enum CurrencyTypeEnum
{
    [EnumMember(Value = "NONE")]
    None = 0,

    /// <summary>
    /// Great Britain Pounds (Fiat).
    /// </summary>
    [EnumMember(Value = "GBP")]
    GBP = 1,

    /// <summary>
    /// Euro (Fiat).
    /// </summary>
    [EnumMember(Value = "EUR")]
    EUR = 2,

    /// <summary>
    /// United States Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "USD")]
    USD = 3,

    /// <summary>
    /// Australian Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "AUD")]
    AUD = 4,
    
    /// <summary>
    /// Bulgarian Lev (Fiat).
    /// </summary>
    [Obsolete("Unsupported currency", false)]
    [EnumMember(Value = "BGN")]
    BGN = 5,
    
    /// <summary>
    /// Canadian Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "CAD")]
    CAD = 6,
    
    /// <summary>
    /// Czech Koruna (Fiat).
    /// </summary>
    [EnumMember(Value = "CZK")]
    CZK = 7,
    
    /// <summary>
    /// Danish Krone (Fiat).
    /// </summary>
    [EnumMember(Value = "DKK")]
    DKK = 8,
    
    /// <summary>
    /// Hungarian Forint (Fiat).
    /// </summary>
    [EnumMember(Value = "HUF")]
    HUF = 9,
    
    /// <summary>
    /// Icelandic Krona (Fiat).
    /// </summary>
    [Obsolete("Unsupported currency", false)]
    [EnumMember(Value = "ISK")]
    ISK = 10,
    
    /// <summary>
    /// Swiss Franc (Fiat).
    /// </summary>
    [EnumMember(Value = "CHF")]
    CHF = 11,
    
    /// <summary>
    /// Norwegian Krone (Fiat).
    /// </summary>
    [EnumMember(Value = "NOK")]
    NOK = 12,
    
    /// <summary>
    /// Polish Zloty (Fiat).
    /// </summary>
    [EnumMember(Value = "PLN")]
    PLN = 13,
    
    /// <summary>
    /// Romanian Leu (Fiat).
    /// </summary>
    [EnumMember(Value = "RON")]
    RON = 14,
    
    /// <summary>
    /// United Arab Emirates Dirham (Fiat).
    /// </summary>
    [EnumMember(Value = "AED")]
    AED = 15,
    
    /// <summary>
    /// Chinese Yuan (Renminbi) (Fiat).
    /// </summary>
    [EnumMember(Value = "CNH")]
    CNH = 16,
    
    /// <summary>
    /// Hong Kong Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "HKD")]
    HKD = 17,
    
    /// <summary>
    /// Israeli Shekel (Fiat).
    /// </summary>
    [EnumMember(Value = "ILS")]
    ILS = 18,
    
    /// <summary>
    /// Japanese Yen (Fiat).
    /// </summary>
    [EnumMember(Value = "JPY")]
    JPY = 19,
    
    /// <summary>
    /// Mexican Peso (Fiat).
    /// </summary>
    [EnumMember(Value = "MXN")]
    MXN = 20,
    
    /// <summary>
    /// New Zealand Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "NZD")]
    NZD = 21,
    
    /// <summary>
    /// Saudi Riyal (Fiat).
    /// </summary>
    [EnumMember(Value = "SAR")]
    SAR = 22,
    
    /// <summary>
    /// Swedish Krona (Fiat).
    /// </summary>
    [EnumMember(Value = "SEK")]
    SEK = 23,
    
    /// <summary>
    /// Singapore Dollar (Fiat).
    /// </summary>
    [EnumMember(Value = "SGD")]
    SGD = 24,
    
    /// <summary>
    /// Turkish Lira (Fiat).
    /// </summary>
    [EnumMember(Value = "TRY")]
    TRY = 25,
    
    /// <summary>
    /// South African Rand (Fiat).
    /// </summary>
    [EnumMember(Value = "ZAR")]
    ZAR = 26,
    
    // Start non-fiat currencies from 1000 to avoid conflicting with supplier mappings.

    /// <summary>
    /// Bitcoin.
    /// </summary>
    [EnumMember(Value = "BTC")]
    BTC = 1001
}