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
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum MoneyMoovResources
{
    accounts,

    merchants,

    metadata,

    openbanking,

    paymentrequests,

    tokens,

    user,

    userroles
}

public static class MoneyMoovUrlBuilder
{
    public const string DEFAULT_MONEYMOOV_BASE_URL = "https://api.nofrixion.com/api/v1/";

    public const string SANDBOX_MONEYMOOV_BASE_URL = "https://api-sandbox.nofrixion.com/api/v1/";

    /// <summary>
    /// Available endpoint URLs for the Accounts resource.
    /// </summary>
    public static class AccountsApi
    {
        public static string CreateAccountsApiUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/";

        public static string GetAccountsApiUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/";

        public static string GetPayoutsForAccountApiUrl(string moneyMoovBaseUrl, Guid accountID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/{accountID}/payouts";
    }

    /// <summary>
    /// Available endpoint URLs for the Merchants resource.
    /// </summary>
    public static class MerchantsApi
    {
        public static string CreateTokenUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{MoneyMoovResources.tokens}";

        public static string DeleteTokenUrl(string moneyMoovBaseUrl, Guid tokenID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{MoneyMoovResources.tokens}/{tokenID}";

        public static string GetUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}";

        public static string GetTokensUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.tokens}";

        public static string GetUserRolesUrl(string moneyMoovBaseUrl, Guid merchantID)
           => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.userroles}";

        public static string GetAccountsUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.accounts}";

        public static string GetUserInvitesUrl(string moneyMoovBaseUrl, Guid merchantID)
           => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/userinvites";
    }

    /// <summary>
    /// Available endpoint URLs for the Metadata resource.
    /// </summary>
    public static class MetadataApi
    {
        public static string EchoUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.metadata}/echo";

        public static string VersionUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.metadata}/version";

        public static string WhoamiUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.metadata}/whoami";

        public static string WhoamiMerchantUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.metadata}/whoamimerchant";
    }

    /// <summary>
    /// Available endpoint URLs for the Open Banking resources.
    /// </summary>
    public static class OpenBankingApi
    {
        public static string AccountsUrl(string moneyMoovBaseUrl, Guid consentID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/accounts/{consentID}";

        public static string ConsentsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/consents";

        public static string ConsentsUrl(string moneyMoovBaseUrl, Guid consentID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/consents/{consentID}";

        public static string ConsentsAllUrl(string moneyMoovBaseUrl, Guid merchantID, string emailAddress)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/consents/{merchantID}/{emailAddress}";

        public static string TransactionsUrl(string moneyMoovBaseUrl, Guid consentID, string accountID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/transactions/{consentID}/{accountID}";
    }

    /// <summary>
    /// Available endpoint URLs for the PaymentRequests resource.
    /// </summary>
    public static class PaymentRequestsApi
    {
        public static string CreateUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}";

        //public static string DeleteUrl(string moneyMoovBaseUrl, Guid paymentRequestID)
        //   => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/{paymentRequestID}";

        public static string GetByIDUrl(string moneyMoovBaseUrl, Guid paymentRequestID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/{paymentRequestID}";

        public static string GetByOrderIDUrl(string moneyMoovBaseUrl, string orderID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/getbyorderid/{orderID}";
    }

    /// <summary>
    /// Available endpoint URLs for the User resource.
    /// </summary>
    public static class UserApi
    {
        public static string SendInviteApiUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.user}/sendinvite/{merchantID}";
    }

    public static string AccountsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/";
    }

    public static string AccountStatementApiUrl(string moneyMoovBaseUrl, string? accountId = null)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{accountId ?? "#accountid#"}/{MoneyMoovApiEndPoints.ACCOUNT_STATEMENT_ENDPOINT}";
    }

    public static string MerchantsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANTS_ENDPOINT}";
    }

    public static string MerchantTransactionsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{MoneyMoovApiEndPoints.MERCHANT_TRANSACTIONS_ENDPOINT}";
    }

    public static string PaymentAccountTransactionsApiUrl(string moneyMoovBaseUrl, string accountId)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.TRANSACTIONS_ENDPOINT}/{accountId}";
    }

    public static string PaymentAccountsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}";
    }

    public static string PaymentAccountTransferApiUrl(string moneyMoovBaseUrl)
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

    public static string UserRolesApiUrl(string moneyMoovBaseUrl, Guid merchantID)
    {
        var url = $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_GET_USERROLES_ENDPOINT}";

        if (merchantID != Guid.Empty)
        {
            url = url.Replace(MoneyApiEndPointParameters.MERCHANT_ID_PARAMETER, merchantID.ToString());
        }

        return url;
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

    public static string BeneficiariesGetAllApiUrl(string moneyMoovBaseUrl, Guid merchantID)
    {
        var url = $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.BENEFICIARIES_GETALL_ENDPOINT}";

        if (merchantID != Guid.Empty)
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
}
