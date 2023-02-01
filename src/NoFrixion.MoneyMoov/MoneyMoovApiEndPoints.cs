//-----------------------------------------------------------------------------
// Filename: MoneyMoovApiEndPoints.cs
//
// Description: List of the MoneyMoov API endpoints.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 May 2022  Aaron Clauson   Created, Carmichael House, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class MoneyApiEndPointParameters
{
    /// <summary>
    /// Url parameter that acts as a place-holder for a merchant ID.
    /// </summary>
    public const string MERCHANT_ID_PARAMETER = "{merchantid}";
}

/// <summary>
/// Convenience class to hold the MoneyMoov API endpoints to save them being hard coded
/// in multiple places or requiring a configuration file setting.
/// </summary>
public static class MoneyMoovApiEndPoints
{
    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Accounts API.
    /// </summary>
    public const string ACCOUNTS_ENDPOINT = "accounts";

    /// <summary>
    /// The URL to access the NoFrixion MoneyMoov account statement API.
    /// </summary>
    public const string ACCOUNT_STATEMENT_ENDPOINT = "statement";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Customer API.
    /// </summary>
    public const string CUSTOMER_ENDPOINT = "customer";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Merchants API.
    /// </summary>
    public const string MERCHANTS_ENDPOINT = "merchants";

    /// <summary>
    /// The URL for the NoFrixion MoneyMoov get failed notifications API.
    /// </summary>
    public const string FAILED_NOTIFICATIONS = "webhooks/failed";

    /// <summary>
    /// The URL for the NoFrixion MoneyMoov merchant get tokens API.
    /// </summary>
    public const string MERCHANT_GET_TOKEN_ENDPOINT = "merchants/{merchantid}/tokens";

    /// <summary>
    /// The URL for the NoFrixion MoneyMoov merchant get user roles API.
    /// </summary>
    public const string MERCHANT_GET_USERROLES_ENDPOINT = "merchants/{merchantid}/userroles";

    /// <summary>
    /// The URL to acquire a strong token that can be used to approve a payout.
    /// </summary>
    public const string MERCHANT_PAYOUT_ACQUIRE_TOKEN_ENDPOINT = "payouts/acquiretoken";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Payout API.
    /// </summary>
    public const string MERCHANT_PAYOUT_ENDPOINT = "payouts";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Payout Submit API.
    /// </summary>
    public const string MERCHANT_PAYOUT_SUBMIT_ENDPOINT = "payouts/submit";

    /// <summary>
    /// The URL for the NoFrixion MoneyMoov merchant create and delete token API.
    /// </summary>
    public const string MERCHANT_TOKEN_ENDPOINT = "merchants/tokens";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov MerchantTransactions API.
    /// </summary>
    public const string MERCHANT_TRANSACTIONS_ENDPOINT = "transactions";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Transfer API.
    /// </summary>
    public const string TRANSFER_ENDPOINT = "payouts/transfer";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov User API.
    /// </summary>
    public const string USER_ENDPOINT = "user";

    /// <summary>
    /// The URL to get all the beneficiaries for a merchant end point.
    /// </summary>
    public const string BENEFICIARIES_GETALL_ENDPOINT = $"merchants/{MoneyApiEndPointParameters.MERCHANT_ID_PARAMETER}/beneficiaries";

    /// <summary>
    /// The URL to for operations on a single beneficiary end point.
    /// </summary>
    public const string BENEFICIARIES_ENDPOINT = $"beneficiaries";

    /// <summary>
    /// The URL to get all the beneficiary groups for a merchant end point.
    /// </summary>
    public const string BENEFICIARY_GROUPS_GETALL_ENDPOINT = $"merchants/{MoneyApiEndPointParameters.MERCHANT_ID_PARAMETER}/beneficiarygroups";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov Transactions API.
    /// </summary>
    public const string TRANSACTIONS_ENDPOINT = "transactions";

    /// <summary>
    /// The URL to access account statement generation actions.
    /// </summary>
    public const string STATEMENTS_ENDPOINT = "statement";
}
