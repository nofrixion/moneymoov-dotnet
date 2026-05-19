//  -----------------------------------------------------------------------------
//   Filename: PaymentRequestExportTests.cs
// 
//   Description: Unit tests for PaymentRequest CSV export.
// 
//   Author(s):
//   Constantine Nalimov (constantine.nalimov@nofrixion.com)
// 
//   History:
//   13 May 2026  Constantine Nalimov  Created.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;

namespace MoneyMoov.UnitTests.Models;

public class PaymentRequestExportTests
{
    [Fact]
    public void PaymentRequest_CsvExport_IncludesHostedPayCheckoutUrl()
    {
        var hostedPayCheckoutUrl = "https://api.test.nofrixion.com/pay/testmerchant/order123";
        var paymentRequest = new PaymentRequest
        {
            HostedPayCheckoutUrl = hostedPayCheckoutUrl
        };

        var header = paymentRequest.CsvHeader();
        var row = paymentRequest.ToCsvRow();

        Assert.Contains("HostedPayCheckoutUrl", header);
        Assert.Contains(hostedPayCheckoutUrl, row);
        Assert.Equal("HostedPayCheckoutUrl", header.Split(',').Last());
    }

    [Fact]
    public void PaymentRequest_CsvHeader_ColumnCount_Matches_ToCsvRow()
    {
        var paymentRequest = new PaymentRequest();

        var headerColumnCount = PaymentRequestExtensions.GetCsvHeader().Split(',').Length;
        var rowColumnCount = paymentRequest.ToCsvRow().Split(',').Length;

        Assert.Equal(headerColumnCount, rowColumnCount);
    }
}
