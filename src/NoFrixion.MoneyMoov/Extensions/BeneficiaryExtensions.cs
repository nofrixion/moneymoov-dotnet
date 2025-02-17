using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Extensions;

public static class BeneficiaryExtensions
{
    public static string ToCsvRowString(this Beneficiary beneficiary)
    {
        var values = new List<string>
        {
            beneficiary.ID.ToString(),
            beneficiary.MerchantID.ToString(),
            beneficiary.Name,
            beneficiary.Currency.ToString(),
            beneficiary.Destination?.AccountID.ToString() ?? "",
            beneficiary.Destination?.Name ?? "",
            beneficiary.Destination?.InternalAccountName ?? "",
            beneficiary.Destination?.Identifier?.IBAN ?? "",
            beneficiary.Destination?.Identifier?.AccountNumber ?? "",
            beneficiary.Destination?.Identifier?.SortCode ?? "",
            beneficiary.Destination?.Identifier?.BIC ?? "",
            beneficiary.IsEnabled.ToString(),
            beneficiary.Authorisations != null
                ? string.Join(",",
                    beneficiary.Authorisations.Select(auth => $"{auth.User.FirstName} {auth.User.LastName}"))
                : "",
            beneficiary.AuthorisersRequiredCount.ToString(),
            beneficiary.AuthorisersCompletedCount.ToString(),
            beneficiary.AuthenticationMethods != null
                ? string.Join(",", beneficiary.AuthenticationMethods.Select(auth => auth.ToString()))
                : "",
            beneficiary.CreatedByEmailAddress ?? "",
            beneficiary.Inserted.ToString("o"),
            beneficiary.LastUpdated.ToString("o"),
            beneficiary.CreatedBy != null ? $"{beneficiary.CreatedBy.FirstName} {beneficiary.CreatedBy.LastName}" : "",
        };

        // Quote values to handle commas in the data
        return string.Join(",", values.Select(x => x.ToSafeCsvString()));
    }
}