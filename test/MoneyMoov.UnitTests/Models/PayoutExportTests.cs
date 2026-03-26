//  -----------------------------------------------------------------------------
//   Filename: PayoutExportTests.cs
// 
//   Description: Unit tests for Payout CSV export.
// 
//   Author(s):
//   Constantine Nalimov
// 
//   History:
//   05 Mar 2026  Constantine Nalimov  Created.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;

namespace MoneyMoov.UnitTests.Models;

public class PayoutExportTests
{
    [Fact]
    public async Task Payout_ToCsvRow_MatchesSnapshot()
    {
        var payout = new Payout
        {
            ID = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            AccountID = Guid.Parse("22345678-1234-1234-1234-123456789012"),
            MerchantID = Guid.Parse("32345678-1234-1234-1234-123456789012"),
            Amount = 100.50m,
            Currency = CurrencyTypeEnum.EUR,
            Type = AccountIdentifierType.BIC,
            Description = "Test Payout",
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier
                {
                    BIC = "TESTBIC1",
                    IBAN = "TESTIBAN1",
                    Currency = CurrencyTypeEnum.EUR
                },
                Name = "John Doe"
            },
            Status = PayoutStatus.QUEUED,
            Inserted = new DateTimeOffset(2025, 3, 5, 12, 0, 0, TimeSpan.Zero),
            LastUpdated = new DateTimeOffset(2025, 3, 5, 12, 30, 0, TimeSpan.Zero)
        };

        var csvRow = payout.ToCsvRow();
        
        await Verify(csvRow);
    }

    [Fact]
    public void Payout_CsvHeader_ColumnCount_Matches_ToCsvRow()
    {
        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier
                {
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        var header = PayoutExtensions.GetCsvHeader();
        var row = payout.ToCsvRow();

        var headerColumnCount = header.Split(',').Length;
        var rowColumnCount = row.Split(',').Length;

        Assert.Equal(headerColumnCount, rowColumnCount);
    }
}
