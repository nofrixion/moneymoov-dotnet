//  -----------------------------------------------------------------------------
//   Filename: TransactionExtensions.cs
// 
//   Description: Contains extension methods for the Transaction model
// 
//   Author(s):
//   Saurav Maiti (saurav@nofrixion.com)
// 
//   History:
//   20 Feb 2025  Saurav Maiti  Created, Hamilton gardens,
//   Dublin, Ireland.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

using System.Globalization;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class TransactionExtensions
{
    public static string ToCsvRowString(this Transaction transaction)
    {
        var values = new List<string>
        {
            transaction.ID.ToString(),
            transaction.AccountID.ToString(),
            transaction.AccountName ?? "",
            transaction.MerchantID.ToString(),
            transaction.Type.ToString(),
            PaymentAmount.GetRoundedAmount(transaction.Currency, transaction.Amount).ToString(CultureInfo.InvariantCulture),
            transaction.Currency.ToString(),
            transaction.Description,
            transaction.TransactionDate.ToString("o"),
            transaction.Inserted.ToString("o"),
            transaction.YourReference ?? "",
            transaction.TheirReference ?? "",
            transaction.CounterpartySummary ?? "",
            PaymentAmount.GetRoundedAmount(transaction.Currency, transaction.Balance).ToString(CultureInfo.InvariantCulture),
            transaction.RuleID?.ToString() ?? "",
            transaction.PayoutID?.ToString() ?? "",
            transaction.VirtualIBAN ?? "",
            transaction.Tags != null ? string.Join(",", transaction.Tags.Select(tag => tag.Name)) : "",
            transaction.AccountSequenceNumber.ToString(),
            transaction.PaymentRequestID?.ToString() ?? "",
            transaction.PaymentRequestCustomFields?.Select(kv => $"{kv.Key}: {kv.Value}").Aggregate((a, b) => $"{a}, {b}") ?? string.Empty
        };
        
        // Quote values to handle commas in the data
        return string.Join(",", values.Select(x => x.ToSafeCsvString()));
    }
}