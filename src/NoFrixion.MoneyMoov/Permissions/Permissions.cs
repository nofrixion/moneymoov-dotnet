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
    CanViewAccount = 1L << 7,
    CanViewAccountForPaymentRequests = 1L << 8,
    CanUpdateAccount = 1L << 9,
    
    // Rules
    CanViewRules = 1L << 10,
    CanCreateRules = 1L << 11,
    CanEditRules = 1L << 12,
    CanDeleteRules = 1L << 13,
    CanAuthoriseRules = 1L << 14,
    
    // Transactions
    CanViewTransactions = 1L << 15,
    
    // Statements
    [Obsolete("Use CanExportData instead.")]
    CanCreateStatements = 1L << 16,
    
    [Obsolete("Use CanExportData instead.")]
    CanViewStatements = 1L << 17,
    
    CanExportData = 1L << 18,
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
    CanAuthoriseTokens = 1L << 36,
    
    // Pay runs
    CanCreatePayruns = 1L << 11,
    CanViewPayruns = 1L << 12,
    CanEditPayruns = 1L << 13,
    CanApprovePayruns = 1L << 14,
    CanDeletePayruns = 1L << 15,
    
    // User roles
    CanViewUserRoles = 1L << 16,
    CanDeleteUserRoles = 1L << 17,
    CanAssignUserRoles = 1L << 18,
    
    // Users
    CanViewUsers = 1L << 19,
    
    // Webhooks
    CanViewWebhooks = 1L << 20,
    CanCreateWebhooks = 1L << 21,
    CanDeleteWebhooks = 1L << 22,
    
    // Merchants
    CanUpdateMerchant = 1L << 23,
    
    // Payment requests
    CanCreatePaymentRequests = 1L << 24,
    CanViewPaymentRequests = 1L << 25,
    CanUpdatePaymentRequests = 1L << 26,
    CanDeletePaymentRequests = 1L << 27,
    
    // Mandates
    CanViewMandates = 1L << 28,
    CanCreateMandates = 1L << 29,
    
    // Permissions
    CanViewRoles = 1L << 30,
    CanCreateRoles = 1L << 31, 
    CanEditRoles = 1L << 32,  
    
    // Reports
    CanCreateReports = 1L << 33,
    CanViewReports = 1L << 34,
}

public static class ClaimTypePrefixes
{
    public const string MERCHANT = "merchant";
    public const string ACCOUNT = "account";
}