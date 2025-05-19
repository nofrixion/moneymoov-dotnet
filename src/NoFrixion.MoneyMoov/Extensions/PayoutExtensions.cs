//  -----------------------------------------------------------------------------
//   Filename: PayoutExtensions.cs
// 
//   Description: Contains extension methods for the Payout model
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

public static class PayoutExtensions
{
    public static string ToCsvRowString(this Payout payout)
    {
        var values = new List<string>
        {
            payout.ID.ToString(),
            payout.PayrunID?.ToString() ?? "",
            payout.AccountID.ToString(),
            payout.MerchantID.ToString(),
            payout.UserID?.ToString() ?? "", // Mapped from CreatedByUserID
            payout.ApproverID?.ToString() ?? "",
            payout.TopupPayrunID?.ToString() ?? "",
            payout.Type.ToString(),
            payout.Description,
            payout.Currency.ToString(),
            PaymentAmount.GetRoundedAmount(payout.Currency, payout.Amount).ToString(CultureInfo.InvariantCulture),
            payout.YourReference ?? "",
            payout.TheirReference ?? "",
            payout.CanProcess.ToString(),
            payout.BatchPayoutID?.ToString() ?? "",
            payout.Destination?.AccountID?.ToString() ?? "", // Mapped from DestinationAccountID
            payout.Destination?.Identifier?.IBAN ?? "",
            payout.Destination?.Identifier?.AccountNumber ?? "",
            payout.Destination?.Identifier?.SortCode ?? "",
            payout.Destination?.Name ?? "",
            payout.MerchantTokenDescription ?? "",
            payout.Status.ToString(),
            payout.CurrentUserID?.ToString() ?? "", // Mapped from ExportedByUserID
            payout.ApprovePayoutUrl ?? "", // Mapped from AuthoriseUrl
            payout.CreatedBy ?? "", // Mapped from CreatedByUserName
            payout.CreatedByEmailAddress ?? "",
            payout.Inserted.ToString("o"),
            payout.LastUpdated.ToString("o"),
            payout.SourceAccountName ?? "",
            payout.SourceAccountIban ?? "",
            payout.SourceAccountNumber ?? "",
            payout.SourceAccountSortcode ?? "",
            payout.FormattedSourceAccountAvailableBalance ?? "",
            payout.InvoiceID ?? "",
            payout.Tags != null ? string.Join(",", payout.Tags.Select(tag => tag.Name)) : "",
            payout.Scheduled?.ToString() ?? "",
            payout.ScheduleDate?.ToString("o") ?? "",
            payout.AuthorisersRequiredCount.ToString(),
            payout.AuthorisersCompletedCount.ToString(),
            payout.Authorisations != null
                ? string.Join(",", payout.Authorisations.Where(x=>x.User != null).Select(auth => $"{auth.User.FirstName} {auth.User.LastName}"))
                : "",
            payout.AuthenticationMethods != null
                ? string.Join(",", payout.AuthenticationMethods.Select(auth => auth.ToString()))
                : "",
            payout.Beneficiary?.ID.ToString() ?? "", // Mapped from BeneficiaryID
            payout.PayrunName ?? "",
            payout.PaymentProcessor.ToString(),
            payout.Rule?.ID.ToString() ?? "", // Mapped from RuleID
            payout.Rule?.Name ?? "", // Mapped from RuleName
            payout.PaymentRail.ToString(),
            payout.Nonce ?? "",
            payout.Documents != null ? string.Join(",", payout.Documents.Select(doc => doc.ID)) : "",
            payout.IsSubmitted.ToString(),
            payout.IsFailed.ToString(),
            payout.IsSettled.ToString()
        };

        // Quote values to handle commas in the data
        return string.Join(",", values.Select(x => x.ToSafeCsvString()));
    }
}