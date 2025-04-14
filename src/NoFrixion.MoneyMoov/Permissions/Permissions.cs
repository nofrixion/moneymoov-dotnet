// -----------------------------------------------------------------------------
//  Filename: PermissionEnums.cs
// 
//  Description: Contains enums for the permissions that can be assigned to users.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  12 07 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.Common.Permissions;

[Flags]
public enum AccountPermissions : ulong
{
    None = 0,
    
    // Payouts
    CanViewPayouts = 1,
    CanCreatePayouts = 1L << 1,
    CanEditPayouts = 1L << 2,
    CanDeletePayouts = 1L << 3,
    CanAuthorisePayouts = 1L << 4,
    CanRejectPayouts = 1L << 5,
    CanCreateBatchPayouts = 1L << 6,
    
    // Accounts
    CanViewAccountForPaymentRequests = 1L << 7,
    CanUpdateAccount = 1L << 8,
    
    // Rules
    CanViewRules = 1L << 9,
    CanCreateRules = 1L << 10,
    CanEditRules = 1L << 11,
    CanDeleteRules = 1L << 12,
    CanAuthoriseRules = 1L << 13,
    
    // Transactions
    CanViewTransactions = 1L << 14,
    
    // Statements
    CanExportData = 1L << 15,
}

[Flags]
public enum MerchantPermissions : ulong
{
    None = 0,
    
    // Accounts
    CanCreateAccounts = 1,
    CanArchiveAccounts = 1L << 1,
    CanConnectAccounts = 1L << 2,
    
    // Beneficiaries
    CanViewBeneficiaries = 1L << 3,
    CanDeleteBeneficiaries = 1L << 4,
    CanEditBeneficiaries = 1L << 5,
    CanCreateBeneficiaries = 1L << 6,
    CanAuthoriseBeneficiaries = 1L << 7,
    
    // Tokens
    CanViewTokens = 1L << 8,
    CanCreateTokens = 1L << 9,
    CanArchiveTokens = 1L << 10,
    CanAuthoriseTokens = 1L << 11,
    
    // Pay runs
    CanCreatePayruns = 1L << 12,
    CanViewPayruns = 1L << 13,
    CanEditPayruns = 1L << 14,
    CanApprovePayruns = 1L << 15,
    CanDeletePayruns = 1L << 16,
    
    // User roles
    CanViewUserRoles = 1L << 17,
    CanDeleteUserRoles = 1L << 18,
    CanAssignUserRoles = 1L << 19,
    
    // Users
    CanViewUsers = 1L << 20,
    
    // Webhooks
    CanViewWebhooks = 1L << 21,
    CanCreateWebhooks = 1L << 22,
    CanDeleteWebhooks = 1L << 23,
    
    // Merchants
    CanUpdateMerchant = 1L << 24,
    
    // Payment requests
    CanCreatePaymentRequests = 1L << 25,
    CanViewPaymentRequests = 1L << 26,
    CanUpdatePaymentRequests = 1L << 27,
    CanDeletePaymentRequests = 1L << 28,
    
    // Mandates
    CanViewMandates = 1L << 29,
    CanCreateMandates = 1L << 30,
    
    // Permissions
    CanViewRoles = 1L << 31,
    CanCreateRoles = 1L << 32, 
    CanEditRoles = 1L << 33,  
    
    // Reports
    CanCreateReports = 1L << 34,
    CanViewReports = 1L << 35,
    
    // Xero
    CanCreateXeroConnection = 1L << 36,
    CanViewXeroConnection = 1L << 37,
    CanUpdateXeroConnection = 1L << 38,
    CanDeleteXeroConnection = 1L << 39,
}

public static class ClaimTypePrefixes
{
    public const string MERCHANT = "merchant";
    public const string ACCOUNT = "account";
}