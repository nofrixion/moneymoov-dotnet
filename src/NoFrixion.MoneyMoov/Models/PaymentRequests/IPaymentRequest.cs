//-----------------------------------------------------------------------------
// Filename: IPaymentRequest.cs
//
// Description: An interface to describe the common fields for each of the
// payment request models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 24 Mar 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public interface IPaymentRequest
{
    public decimal Amount { get; set; }

    public CurrencyTypeEnum Currency { get; set; }

    public string? CustomerID { get; set; }

    public string? OrderID { get; set; }

    public PaymentMethodTypeEnum PaymentMethodTypes { get; set; }

    public string? Description { get; set; }

    public Guid? PispAccountID { get; set; }

    public string? BaseOriginUrl { get; set; }

    public string? CallbackUrl { get; set; }

    public string? SuccessWebHookUrl { get; set; }

    public bool CardAuthorizeOnly { get; set; }

    public bool CardCreateToken { get; set; }

    public CardTokenCreateModes CardCreateTokenMode { get; set; }

    public bool IgnoreAddressVerification { get; set; }

    public bool CardIgnoreCVN { get; set; }

    public string? PispRecipientReference { get; set; }

    public string? CustomerEmailAddress { get; set; }

    public PaymentProcessorsEnum PaymentProcessor { get; set; }

    public bool UseHostedPaymentPage { get; set; }

    public string? LightningInvoice { get; set; }
}
