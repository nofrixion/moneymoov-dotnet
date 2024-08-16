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
    CanCreatePayouts = 2,
    CanEditPayouts = 4,
    CanDeletePayouts = 8,
    CanAuthorisePayouts = 16,
    CanRejectPayouts = 32,
    CanCreateBatchPayouts = 64,
    
    // Accounts
    CanViewAccount = 128,
    CanUpdateAccount = 256,
    
    // Rules
    CanViewRules = 512,
    CanCreateRules = 1024,
    CanEditRules = 2048,
    CanDeleteRules = 4096,
    CanAuthoriseRules = 8192,
    
    // Transactions
    CanViewTransactions = 16384,
    
    // Statements
    CanCreateStatements = 32768,
    CanViewStatements = 65536,
}

[Flags]
public enum MerchantPermissions : ulong
{
    None = 0,
    
    // Accounts
    CanViewAccounts = 1,
    CanCreateAccounts = 2,
    
    // Beneficiaries
    CanViewBeneficiaries = 4,
    CanDeleteBeneficiaries = 8,
    CanEditBeneficiaries = 16,
    CanCreateBeneficiaries = 32,
    CanAuthoriseBeneficiaries = 64,
    
    // Tokens
    CanViewTokens = 128,
    CanCreateTokens = 256,
    CanDeleteTokens = 512,
    
    // Pay runs
    CanCreatePayruns = 1024,
    CanViewPayruns = 2048,
    CanEditPayruns = 4096,
    CanApprovePayruns = 8192,
    CanDeletePayruns = 16384,
    
    // Payouts
    CanViewMerchantPayouts = 32768,
    
    // User roles
    CanViewUserRoles = 65536,
    CanDeleteUserRoles = 131072,
    CanAssignUserRoles = 262144,
    
    // Users
    CanViewUsers = 524288,
    CanViewUserInvites = 1048576,
    CanEditUsers = 2097152,
    
    // Rules
    CanViewAllRules = 4194304,
    
    // Webhooks
    CanViewWebhooks = 8388608,
    CanCreateWebhooks = 16777216,
    CanDeleteWebhooks = 33554432,
    
    // Transactions
    CanViewAllTransactions = 67108864,
    
    // Merchants
    CanViewMerchant = 134217728,
    CanUpdateMerchant = 268435456,
    
    // Payment requests
    CanCreatePaymentRequests = 536870912,
    CanViewPaymentRequests = 1073741824,
    CanUpdatePaymentRequests = 2147483648,
    
    // Mandates
    CanViewMandates = 4294967296,
    CanCreateMandates = 8589934592,
    
    // Permissions
    CanViewGroups = 17179869184,
    CanCreateGroups = 34359738368,
    CanEditGroups = 68719476736,
}

[Flags]
public enum UserPermissions : ulong
{
    None = 0,
    
    CanViewMerchants = 1,
    
    CanViewAllAccounts = 2,
    
    CanCreateReports = 4,
    
    CanViewAllAccountsTransactions = 8,
    
    CanViewAllBeneficiaries = 16,
    
    CanViewAllPaymentRequests = 32
}

public static class ClaimTypePrefixes
{
    public const string MERCHANT = "merchant";
    public const string ACCOUNT = "account";
    public const string USER = "user";
}