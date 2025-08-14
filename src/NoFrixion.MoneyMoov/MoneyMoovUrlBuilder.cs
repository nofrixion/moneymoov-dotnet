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
    
    banksettings,

    batch,

    beneficiaries,

    consents,

    merchants,

    metadata,

    openbanking,

    paymentrequests,

    payouts,

    reports,

    rules,

    statements,

    tokens,

    transactions,

    user,

    userinvites,

    userroles,

    webhooks,
    
    roles,

    users,
}


public static class MoneyMoovUrlBuilder
{
    public const string DEFAULT_MONEYMOOV_BASE_URL = "https://api.nofrixion.com/api/v1";

    public const string SANDBOX_MONEYMOOV_BASE_URL = "https://api-sandbox.nofrixion.com/api/v1";

    /// <summary>
    /// Available endpoint URLs for the Accounts resource.
    /// </summary>
    public static class AccountsApi
    {
        public static string AccountUrl(string moneyMoovBaseUrl, Guid accountID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/{accountID}";

        public static string AccountsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/";

        public static string AccountPayoutsUrl(string moneyMoovBaseUrl, Guid accountID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/{accountID}/{MoneyMoovResources.payouts}";
        
        public static string StatementsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/{MoneyMoovResources.statements}";

        public static string StatementsUrlWithAccountPlaceholder(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/##account##/{MoneyMoovResources.statements}";
        
        public static string StatementsUrl(string moneyMoovBaseUrl, Guid accountID, Guid statementID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.accounts}/{accountID}/{MoneyMoovResources.statements}/{statementID}";
    }

    /// <summary>
    /// Available endpoint URLs for the Merchants resource.
    /// </summary>
    public static class MerchantsApi
    {
        public static string MerchantsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}";

        public static string MerchantsUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}";

        public static string AllMerchantsTokensUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{MoneyMoovResources.tokens}";

        public static string MerchantTokenUrl(string moneyMoovBaseUrl, Guid tokenID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{MoneyMoovResources.tokens}/{tokenID}";

        public static string MerchantAccountsUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.accounts}";

        public static string MerchantBankSettings(string moneyMoovBaseUrl, Guid merchantID,
            CurrencyTypeEnum? currency = null)
        {
            return currency.HasValue 
                ? $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.banksettings}?currency={currency}" 
                : $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.banksettings}";
        }
        
        public static string MerchantTokensUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.tokens}";

        public static string MerchantUserInvitesUrl(string moneyMoovBaseUrl, Guid merchantID)
           => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.userinvites}";
        
        public static string MerchantWebHooksUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.webhooks}";
        
        public static string MerchantRolesUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.roles}";

        public static string MerchantRoleUrl(string moneyMoovBaseUrl, Guid merchantID, Guid roleID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.roles}/{roleID}";

        public static string MerchantRoleUsersUrl(string moneyMoovBaseUrl, Guid merchantID, Guid roleID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.roles}/{roleID}/{MoneyMoovResources.users}";
        
        public static string MerchantRoleUsersUrl(string moneyMoovBaseUrl, Guid merchantID, Guid roleID, Guid userID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.roles}/{roleID}/{MoneyMoovResources.users}/{userID}";

        public static string MerchantRolesUserUrl(string moneyMoovBaseUrl, Guid merchantID, Guid userID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.merchants}/{merchantID}/{MoneyMoovResources.users}/{userID}/{MoneyMoovResources.roles}";
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
        public static string AccountUrl(string moneyMoovBaseUrl, Guid accountId)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/account/{accountId}";

        public static string AccountsUrl(string moneyMoovBaseUrl, Guid consentID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/accounts/{consentID}";

        public static string ConsentsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/{MoneyMoovResources.consents}";

        public static string ConsentsUrl(string moneyMoovBaseUrl, Guid consentID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/{MoneyMoovResources.consents}/{consentID}";

        public static string ConsentsAllUrl(string moneyMoovBaseUrl, Guid merchantID, string emailAddress)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/{MoneyMoovResources.consents}/{merchantID}/{emailAddress}";

        public static string TransactionsUrl(string moneyMoovBaseUrl, Guid consentID, string accountID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.openbanking}/transactions/{consentID}/{accountID}";
    }

    /// <summary>
    /// Available endpoint URLs for the PaymentRequests resource.
    /// </summary>
    public static class PaymentRequestsApi
    {
        public static string PaymentRequestsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}";

        public static string PaymentRequestUrl(string moneyMoovBaseUrl, Guid paymentRequestID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/{paymentRequestID}";

        public static string GetByOrderIDUrl(string moneyMoovBaseUrl, string orderID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/getbyorderid/{orderID}";
        
        public static string BatchPaymentRequestsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.paymentrequests}/batchcreate";
    }

    /// <summary>
    /// Available endpoint URLs for the Payouts resource.
    /// </summary>
    public static class PayoutsApi
    {
        public static string PayoutUrl(string moneyMoovBaseUrl, Guid payoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/{payoutID}";

        public static string PayoutsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}";

        public static string BulkPayoutsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/bulk";

        public static string GetByInvoiceIDUrl(string moneyMoovBaseUrl, string invoiceID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/getbyinvoiceid/{invoiceID}";

        public static string SubmitPayoutUrl(string moneyMoovBaseUrl, Guid payoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/submit/{payoutID}";

        public static string BatchPayoutUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/{MoneyMoovResources.batch}";

        public static string BatchPayoutUrl(string moneyMoovBaseUrl, Guid batchPayoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/{MoneyMoovResources.batch}/{batchPayoutID}";

        public static string SubmitBatchPayoutUrl(string moneyMoovBaseUrl, Guid payoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/{MoneyMoovResources.batch}/submit/{payoutID}";

        public static string CancelScheduledPayoutUrl(string moneyMoovBaseUrl, Guid payoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/cancel/{payoutID}";
        
        public static string RejectPayoutUrl(string moneyMoovBaseUrl, Guid payoutID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/reject/{payoutID}";

        public static string SendPayoutUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.payouts}/send";
    }

    /// <summary>
    /// Available endpoint URLs for the Reports resource.
    /// </summary>
    public static class ReportsApi
    {
        public static string ReportResultUrl(string moneyMoovBaseUrl, Guid reportID, int? statementNumber)
            => statementNumber != null ?
                $"{moneyMoovBaseUrl}/{MoneyMoovResources.reports}/{reportID}/result/{statementNumber}" :
                $"{moneyMoovBaseUrl}/{MoneyMoovResources.reports}/{reportID}/result";
    }

    /// <summary>
    /// Available endpoint URLs for the Rules resource.
    /// </summary>
    public static class RulesApi
    {
        public static string RuleUrl(string moneyMoovBaseUrl, Guid ruleID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.rules}/{ruleID}";

        public static string RulesUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.rules}";

        public static string ApproveRuleUrl(string moneyMoovBaseUrl, Guid ruleID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.rules}/{ruleID}/approve";
    }

    /// <summary>
    /// Available endpoint URLs for the Statements resource.
    /// </summary>
    public static class StatementsApi
    {
        public static string StatementsUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.statements}";

        public static string StatementsUrl(string moneyMoovBaseUrl, Guid statementID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.statements}/{statementID}";
    }

    /// <summary>
    /// Available endpoint URLs for the Tokens resource.
    /// </summary>
    public static class TokensApi
    {
        public static string TokensUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.tokens}";

        public static string TokenUrl(string moneyMoovBaseUrl, Guid tokenID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.tokens}/{tokenID}";

        public static string AuthoriseTokenUrl(string moneyMoovBaseUrl, Guid tokenID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.tokens}/authorise/{tokenID}";
    }

    /// <summary>
    /// Available endpoint URLs for the User resource.
    /// </summary>
    public static class UserApi
    {
        public static string UserApiUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.user}";

        public static string UserApiUrl(string moneyMoovBaseUrl, Guid userID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.user}/{userID}";
    }

    /// <summary>
    /// Available endpoint URLs for the User Invites resource.
    /// </summary>
    public static class UserInvitesApi
    {
        public static string UserInvitesUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.userinvites}";

        public static string UserInviteUrl(string moneyMoovBaseUrl, Guid userInviteID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.userinvites}/{userInviteID}";

        public static string AuthoriseUserInviteUrl(string moneyMoovBaseUrl, Guid userInviteID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.userinvites}/authorise/{userInviteID}";

        public static string UserInviteDetailsUrl(string moneyMoovBaseUrl, Guid userInviteID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.userinvites}/{userInviteID}/details";
        
        public static string BatchUserInvitesUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.userinvites}/batchcreate";
    }

    /// <summary>
    /// Available endpoint URLs for the merchant webhooks resource.
    /// </summary>
    public static class WebhooksApi
    {
        public static string WebhookUrl(string moneyMoovBaseUrl, Guid id)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.webhooks}/{id}";

        public static string WebhooksUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.webhooks}";

        public static string AllWebhooksUrl(string moneyMoovBaseUrl, Guid merchantID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.webhooks}/{merchantID}";
    }

    public static class BeneficiariesApi
    {
        public static string BeneficiaryUrl(string moneyMoovBaseUrl, Guid beneficiaryID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.beneficiaries}/{beneficiaryID}";

        public static string BeneficiaryUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.beneficiaries}";

        public static string AuthoriseBeneficiaryUrl(string moneyMoovBaseUrl, Guid beneficiaryID)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.beneficiaries}/authorise/{beneficiaryID}";
        
        public static string BatchBeneficiariesUrl(string moneyMoovBaseUrl)
            => $"{moneyMoovBaseUrl}/{MoneyMoovResources.beneficiaries}/batchcreate";
    }

    public static string AccountStatementApiUrl(string moneyMoovBaseUrl, string? accountId = null)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.ACCOUNTS_ENDPOINT}/{accountId ?? "#accountid#"}/{MoneyMoovApiEndPoints.ACCOUNT_STATEMENT_ENDPOINT}";
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

    public static string MerchantPayoutApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.MERCHANT_PAYOUT_ENDPOINT}";
    }

    public static string CustomerApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/{MoneyMoovApiEndPoints.CUSTOMER_ENDPOINT}";
    }

    public static string PaymentRequestsApiUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/paymentrequests";
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

    public static string BusinessHubUrl(string moneyMoovBaseUrl)
    {
        return $"{moneyMoovBaseUrl}/signalr/business";
    } 
}
