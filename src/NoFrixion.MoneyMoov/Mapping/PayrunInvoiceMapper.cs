//-----------------------------------------------------------------------------
// Filename: PayrunInvoiceMapper.cs
//
// Description: Payrun Invoice model mapper.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Jul 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Ardalis.GuardClauses;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov;

public static class PayrunInvoiceMapper
{
    /// <summary>
    /// A safe TheirReference value that will pass validation sofr all currencies and processors.
    /// Payrun invoices result in an auto-generating TheirReference when teh Payout is ultimately
    /// created.
    /// </summary>
    private const string SAFE_THEIR_REFERENCE = "Safe Reference";

    public static Payout ToPayout(this PayrunInvoice invoice)
    {
        Guard.Against.Null(invoice, nameof(invoice));

        return new Payout
        {
            Currency = invoice.Currency,
            Type = invoice.Currency == CurrencyTypeEnum.GBP ? AccountIdentifierType.SCAN : AccountIdentifierType.IBAN,
            Amount = invoice.TotalAmount,
            TheirReference = !string.IsNullOrWhiteSpace(invoice.PaymentReference)
                ? invoice.PaymentReference
                : SAFE_THEIR_REFERENCE,
            Destination = invoice.Destination
        };
    }
}