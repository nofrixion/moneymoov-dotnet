//-----------------------------------------------------------------------------
// Filename: MoneyMoovUrlBuilder.cs
//
// Description: Route builder for the MooneyMoov Portal.
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 15 Oct 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 09 Jul 2022  Aaron Clauson    Renamed from MoneyMoovUrlBuilder to MoneyMoovUrlBuilder.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class MoneyMoovUrlBuilder
{
    public const string DEFAULT_MONEYMOOV_BASE_URL = "https://api.nofrixion.com/api/v1/";

    public const string SANDBOX_MONEYMOOV_BASE_URL = "https://api-sandbox.nofrixion.com/api/v1/";

    public static string AccountsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/";
    }

    public static string AccountStatementApiUrl(string moneyMoovBaseUrl, string? accountId = null)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{accountId ?? "#accountid#"}/{MoneyMoovApiEndPoints.ACCOUNT_STATEMENT_ENDPOINT}";
    }

    public static string AccountPendingPayoutsApiUrl(string moneyMoovBaseUrl, string? accountId = null)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{accountId ?? "#accountid#"}/pending-payouts";
    }

    public static string MerchantsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANTS_ENDPOINT}";
    }

    public static string MerchantTransactionsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{MoneyMoovApiEndPoints.MERCHANT_TRANSACTIONS_ENDPOINT}";
    }

    public static string MerchantAccountTransactionsApiUrl(string moneyMoovBaseUrl, string accountId)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.TRANSACTIONS_ENDPOINT}/{accountId}";
    }

    public static string MerchantAccountsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}";
    }

    public static string MerchantAccountTransferApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.TRANSFER_ENDPOINT}";
    }

    public static string MerchantPayoutApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_PAYOUT_ENDPOINT}";
    }

    public static string MerchantPayoutAcquireTokenApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_PAYOUT_ACQUIRE_TOKEN_ENDPOINT}";
    }

    public static string MerchantPayoutSubmitApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_PAYOUT_SUBMIT_ENDPOINT}";
    }

    public static string UserApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.USER_ENDPOINT}";
    }

    public static string CustomerApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.CUSTOMER_ENDPOINT}";
    }

    public static string UserTokenApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.USER_ENDPOINT}/tokens";
    }

    public static string UserTokenAcquireApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.USER_ENDPOINT}/tokensacquire";
    }

    public static string UserTokensByTypeApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.USER_ENDPOINT}/tokens";
    }

    public static string UserMerchantsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/merchants";
    }

    public static string UserSettingsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.USER_ENDPOINT}/settings";
    }

    public static string UserMerchantGetTokensApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_GET_TOKEN_ENDPOINT}";
    }

    public static string UserRolesApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANTS_ENDPOINT}/userroles";
    }

    /// <summary>
    /// The tokens API URL is used for deleting and creating merchant tokens. It's different
    /// to the GET API which requires the merchant ID in the URL. This is inconsistent but perhaps more
    /// usable. One to watch for end user feedback.
    /// </summary>
    public static string UserMerchantTokensApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_TOKEN_ENDPOINT}";
    }

    public static string FailedNotificationsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.FAILED_NOTIFICATIONS}";
    }

    public static string PaymentRequestsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/paymentrequests";
    }

    public static string HostedPaymentResultUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/pay/result";
    }

    /// <summary>
    /// The MoneyMoov URL to retrieve the current version of the API.
    /// </summary>
    /// <returns>A URL for the MoneyMoov version end point.</returns>
    public static string VersionUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/metadata/version";
    }

    /// <summary>
    /// The MoneyMoov URL to check the identity of a request's access token.
    /// </summary>
    /// <returns>A URL for the MoneyMoov whoami end point.</returns>
    public static string WhoamiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/metadata/whoami";
    }

    /// <summary>
    /// The MoneyMoov URL to check the identity of a request's merchant access token.
    /// </summary>
    /// <returns>A URL for the MoneyMoov whoami end point.</returns>
    public static string WhoamiMerchantUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/metadata/whoamimerchant";
    }

    public static string BeneficiariesGetAllApiUrl(string moneyMoovBaseUrl, Guid merchantID)
    {
        var url = $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.BENEFICIARIES_GETALL_ENDPOINT}";

        if(merchantID != Guid.Empty)
        {
            url = url.Replace(MoneyApiEndPointParameters.MERCHANT_ID_PARAMETER, merchantID.ToString());
        }

        return url;
    }

    public static string BeneficiariesApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.BENEFICIARIES_ENDPOINT}";
    }

    public static string BeneficiaryGroupsGetAllApiUrl(string moneyMoovBaseUrl, Guid merchantID)
    {
        var url = $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.BENEFICIARY_GROUPS_GETALL_ENDPOINT}";

        if (merchantID != Guid.Empty)
        {
            url = url.Replace(MoneyApiEndPointParameters.MERCHANT_ID_PARAMETER, merchantID.ToString());
        }

        return url;
    }

    /// <summary>
    /// The MoneyMoov URL to echo a request message.
    /// </summary>
    /// <returns>A URL for the MoneyMoov echo end point.</returns>
    public static string EchoUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/metadata/echo";
    }
}
