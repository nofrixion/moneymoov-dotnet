// -----------------------------------------------------------------------------
//  Filename: MerchantTokenPermissionsEnum.cs
// 
//  Description: Enum for permissions associated with merchant tokens.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  14 02 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
//  09 01 2024  Aaron Clauson   Added report permissions.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

[Flags]
public enum MerchantTokenPermissionsEnum
{
    /// <summary>
    /// Permission to deny all actions. This won't be assigned in practice but when
    /// checking whether a permission is granted, this can be used to specify that merchant
    /// tokens are not allowed to perform any actions.
    /// </summary>
    Deny = 0,

    /// <summary>
    /// Permission to create a payment request
    /// </summary>
    CreatePaymentRequest = 1,

    /// <summary>
    /// Permission to edit a payment request
    /// </summary>
    EditPaymentRequest = 1 << 1,

    /// <summary>
    /// Permission to delete a payment request
    /// </summary>
    DeletePaymentRequest = 1 << 2,

    /// <summary>
    /// Permission to create a rule
    /// </summary>
    CreateRule = 1 << 3,

    /// <summary>
    /// Permission to edit a rule
    /// </summary>
    EditRule = 1 << 4,

    /// <summary>
    /// Permission to delete a rule
    /// </summary>
    DeleteRule = 1 << 5,

    /// <summary>
    /// Permission to create a payout
    /// </summary>
    CreatePayout = 1 << 6,

    /// <summary>
    /// Permission to edit a payout
    /// </summary>
    EditPayout = 1 << 7,

    /// <summary>
    /// Permission to delete a payout
    /// </summary>
    DeletePayout = 1 << 8,

    /// <summary>
    /// Permission to create a report.
    /// </summary>
    CreateReport = 1 << 9,

    /// <summary>
    /// Permission to edit, retrieve and initiate a report.
    /// </summary>
    EditReport = 1 << 10,

    /// <summary>
    /// Permission to delete a report.
    /// </summary>
    DeleteReport = 1 << 11,

    /// <summary>
    /// Permission to execute and get report results.
    /// </summary>
    ExecuteReport = 1 << 12,

    /// <summary>
    /// Permission to create a new payment account.
    /// </summary>
    CreatePaymentAccount = 1 << 13,

    /// <summary>
    /// Permission to edit details on a payment account.
    /// </summary>
    EditPaymentAccount = 1 << 14,

    /// <summary>
    /// Permission to create and submit a payout from a trusted source.
    /// </summary>
    TrustedSubmitPayout = 1 << 15,

    /// <summary>
    /// Permission to perform open banking account information actions.
    /// </summary>
    OpenBankingAccountInformation = 1 << 16,

    /// <summary>
    /// Permission to create a direct debit mandate.
    /// </summary>
    CreateDirectDebitMandate = 1 << 17,

    /// <summary>
    /// Permission to submit a direct debit payment attempt.
    /// </summary>
    SubmitDirectDebitPayment = 1 << 18
}