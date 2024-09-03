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
    CanCreateStatements = 1L << 15,
    CanViewStatements = 1L << 16,
}

[Flags]
public enum MerchantPermissions : ulong
{
    None = 0,
    
    // Accounts
    CanViewAccounts = 1,
    CanCreateAccounts = 1L << 1,
    CanArchiveAccounts = 1L << 37,
    
    // Beneficiaries
    CanViewBeneficiaries = 1L << 2,
    CanDeleteBeneficiaries = 1L << 3,
    CanEditBeneficiaries = 1L << 4,
    CanCreateBeneficiaries = 1L << 5,
    CanAuthoriseBeneficiaries = 1L << 6,
    
    // Tokens
    CanViewTokens = 1L << 7,
    CanCreateTokens = 1L << 8,
    CanDeleteTokens = 1L << 9,
    
    // Pay runs
    CanCreatePayruns = 1L << 10,
    CanViewPayruns = 1L << 11,
    CanEditPayruns = 1L << 12,
    CanApprovePayruns = 1L << 13,
    CanDeletePayruns = 1L << 14,
    
    // Payouts
    CanViewMerchantPayouts = 1L << 15,
    
    // User roles
    CanViewUserRoles = 1L << 16,
    CanDeleteUserRoles = 1L << 17,
    CanAssignUserRoles = 1L << 18,
    
    // Users
    CanViewUsers = 1L << 19,
    CanViewUserInvites = 1L << 20,
    CanEditUsers = 1L << 21,
    
    // Rules
    CanViewAllRules = 1L << 22,
    
    // Webhooks
    CanViewWebhooks = 1L << 23,
    CanCreateWebhooks = 1L << 24,
    CanDeleteWebhooks = 1L << 25,
    
    // Transactions
    CanViewAllTransactions = 1L << 26,
    
    // Merchants
    CanViewMerchant = 1L << 27,
    CanUpdateMerchant = 1L << 28,
    
    // Payment requests
    CanCreatePaymentRequests = 1L << 29,
    CanViewPaymentRequests = 1L << 30,
    CanUpdatePaymentRequests = 1L << 31,
    
    // Mandates
    CanViewMandates = 1L << 32,
    CanCreateMandates = 1L << 33,
    
    // Permissions
    CanViewRoles = 1L << 34,
    CanCreateRoles = 1L << 35, 
    CanEditRoles = 1L << 36,  
}

[Flags]
public enum UserPermissions : ulong
{
    None = 0,
    
    CanCreateReports = 1,
    
    //CanViewAllBeneficiaries = 1L << 1,
    
    CanViewAllPaymentRequests = 1L << 2,
}

public static class ClaimTypePrefixes
{
    public const string MERCHANT = "merchant";
    public const string ACCOUNT = "account";
    public const string USER = "user";
}