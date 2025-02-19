//  -----------------------------------------------------------------------------
//   Filename: PaymentAccountExtensions.cs
// 
//   Description: Contains extension methods for the PaymentAccount model
// 
//   Author(s):
//   Saurav Maiti (saurav@nofrixion.com)
// 
//   History:
//   18 Feb 2025  Saurav Maiti  Created, Hamilton gardens,
//   Dublin, Ireland.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

using System.Globalization;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class PaymentAccountExtensions
{
    public static string ToCsvRowString(this PaymentAccount account)
    {
        var values = new List<string>
        {
            account.ID.ToString(),
            account.AccountName,
            account.AccountSupplierName.ToString(),
            account.IsConnectedAccount.ToString(),
            PaymentAmount.GetRoundedAmount(account.Currency, account.Balance).ToString(CultureInfo.InvariantCulture),
            PaymentAmount.GetRoundedAmount(account.Currency, account.SubmittedPayoutsBalance).ToString(CultureInfo.InvariantCulture),
            PaymentAmount.GetRoundedAmount(account.Currency, account.AvailableBalance).ToString(CultureInfo.InvariantCulture),
            account.Currency.ToString(),
            account.Identifier?.IBAN ?? "",
            account.Identifier?.SortCode ?? "",
            account.Identifier?.AccountNumber ?? "",
            account.Identifier?.BIC ?? "",
            account.Inserted.ToString("o"),
            account.LastUpdated.ToString("o"),
            account.IsDefault.ToString(),
            account.BankName ?? "",
            account.ExpiryDate?.ToString("o") ?? "",
            account.XeroBankFeedConnectionStatus?.ToString() ?? "",
            account.XeroBankFeedSyncStatus.ToString(),
            account.XeroBankFeedLastSyncedAt?.ToString("o") ?? "",
            account.XeroBankFeedSyncLastFailedAt?.ToString("o") ?? "",
            account.XeroBankFeedSyncLastFailureReason ?? "",
            account.XeroUnsynchronisedTransactionsCount?.ToString() ?? "",
            account.DefaultPaymentRail.ToString(),
            account.IsArchived.ToString(),
            account.SupplierSepaInstantStatus?.ToString() ?? ""
        };

        // Quote values to handle commas in the data
        return string.Join(",", values.Select(x => x.ToSafeCsvString()));
    }
}