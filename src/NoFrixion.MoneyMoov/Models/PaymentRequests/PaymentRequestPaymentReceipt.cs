//-----------------------------------------------------------------------------
// Filename: PaymentRequestPaymentReceipt.cs
//
// Description: The model used to generate a payment receipt pdf and send email.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 07 May 2025  Saurav Maiti  Created, Hamilton gardens, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestPaymentReceipt
{
    public required Guid PaymentRequestID { get; set; }
    
    public required string GeneratedOn { get; set; }

    public required string MerchantName { get; set; }
    
    public string? MerchantShortName { get; set; }
    
    public string? MerchantLogoUrlPng { get; set; }
    
    public string? MerchantLogoUrlSvg { get; set; }

    public required string MerchantDisplayAddress { get; set; }

    public required string TotalAmount { get; set; }

    public required CurrencyTypeEnum Currency { get; set; }

    public required string AmountOutstanding { get; set; }

    public required string TotalAmountPaid { get; set; }

    public List<PaymentRequestCustomField>? CustomFields { get; set; }

    public string? CustomerName { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
    
    public string? CustomerEmailAddress { get; set; }

    public bool ShowDescriptionInEmail => !string.IsNullOrWhiteSpace(Description) ||
                                          !string.IsNullOrWhiteSpace(Title);
    
    public required List<PaymentRequestSuccessfulPayment> SuccessfulPayments { get; set; }
    
    public string? LastPaymentDate { get; set; }
    
    public string? LastPaymentMethod { get; set; }
    
    public string? LastPaymentAmount { get; set; }
    
    public Guid? LastPaymentRequestEventID { get; set; }
    
    public List<PaymentRequestFieldDisplaySetting>? FieldDisplaySettings { get; set; }
}

public class PaymentRequestSuccessfulPayment
{
    public required string PaymentDate { get; set; }

    public required string AmountPaid { get; set; }

    public required string PaymentMethod { get; set; }
}