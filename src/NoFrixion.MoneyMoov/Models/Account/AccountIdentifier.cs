﻿// -----------------------------------------------------------------------------
//  Filename: AccountIdentifier.cs
// 
//  Description: Account identifier:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class AccountIdentifier
{
    /// <summary>
    /// The type of the account identifier.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public AccountIdentifierType Type
    {
        get
        {
            if (!string.IsNullOrEmpty(IBAN))
            {
                return AccountIdentifierType.IBAN;
            }

            if (!string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(AccountNumber))
            {
                return AccountIdentifierType.SCAN;
            }

            // Return default
            return AccountIdentifierType.Unknown;
        }
    }

    /// <summary>
    /// The currency for the account.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// The Bank Identifier Code for an IBAN.
    /// </summary>
    public string BIC { get; set; }

    /// <summary>
    /// The International Bank Account Number for the identifier. Only applicable 
    /// for IBAN identifiers.
    /// </summary>
    public string IBAN { get; set; }

    /// <summary>
    /// The account Sort Code. Only applicable for SCAN identifiers.
    /// </summary>
    public string SortCode { get; set; }

    /// <summary>
    /// Bank account number. Only applicable for SCAN identifiers.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string Summary =>   
        Type == AccountIdentifierType.IBAN ? Type.ToString() + ": " + IBAN :
        Type == AccountIdentifierType.SCAN ? Type.ToString() + ": " + SortCode + " / " + AccountNumber :
         "Unknown account identifier type.";
}