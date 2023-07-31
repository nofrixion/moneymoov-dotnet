//-----------------------------------------------------------------------------
// Filename: CsvMapperTests.cs
//
// Description: Unit tests for the MoneyMoov CsvMapper class.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Jul 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class CsvMapperTests : MoneyMoovUnitTestBase<CsvMapperTests>
{
    public CsvMapperTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Test that a minimal csv file creates the expected models.
    /// </summary>
    [Fact]
    public void Minimal_Payment_Request_Create_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string csv =
@"Amount,Currency
0.42,EUR
0.43,GBP";

        var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Amount", "{Amount}"},
            { "Currency", "{Currency}" }
        };

        using var reader = new StringReader(csv);

        var results = CsvMapper.MapToModel<PaymentRequestCreate>(reader, mapping).ToList();

        Assert.NotNull(results);
        Assert.Equal(2, results.Count());
        Assert.Equal(0.42M, results[0].Model.Amount);
        Assert.Equal(CurrencyTypeEnum.EUR, results[0].Model.Currency);
        Assert.Equal("0.42,EUR", results[0].CsvRow.Trim());
        Assert.True(results[0].Problem.IsEmpty);
        Assert.Equal(0.43M, results[1].Model.Amount);
        Assert.Equal(CurrencyTypeEnum.GBP, results[1].Model.Currency);
        Assert.Equal("0.43,GBP", results[1].CsvRow.Trim());
        Assert.True(results[1].Problem.IsEmpty);
    }

    /// <summary>
    /// Test that a csv file from car dealer customer creates the expected model.
    /// </summary>
    [Fact]
    public void CarDealer_Payment_Request_Create_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string csv =
@"Company,Reference,First Name,Surname,E-mail Address,CURRENCY,Document Number,Document Date,Base Value,Notification E-mail,KDB Time Stamp,Description,Registration Number
Company X,Ms Jane Doe,Jane, Doe,blackhole@nofrixion.com,EUR,5001834,26/07/2023,39400,blackhole@nofrixion.com,2023-07-26 07:47:14.295,RANGE ROVER TDV8,001XX3
";

        var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "MerchantID","B7DC7E46-C098-419D-58D6-08DAE4F90915" },
            { "Amount", "{Base Value}" },
            { "Currency", "{CURRENCY}" },
            { "OrderID", "{Document Number}" },
            { "PaymentMethodTypes", "pisp" },
            { "Description", "{Description}" },
            { "ShippingFirstName", "{First Name}" },
            { "ShippingLastName", "{Surname}" },
            { "First Name", "ShippingFirstName" },
            { "Surname", "ShippingLastName" },
            { "CustomerEmailAddress", "{E-mail Address}" },
            { "PartialPaymentMethod", "partial" },
            { "UseHostedPaymentPage", "true" },
            { "Title", "{Description}" }
        };

        using var reader = new StringReader(csv);

        var results = CsvMapper.MapToModel<PaymentRequestCreate>(reader, mapping).ToList();

        Assert.NotNull(results);
        Assert.Single(results);

        Logger.LogDebug(results[0].CsvRow);
        Logger.LogDebug(results[0].Problem.ToTextErrorMessage());

        Assert.True(results[0].Problem.IsEmpty);
        Assert.Equal(Guid.Parse("B7DC7E46-C098-419D-58D6-08DAE4F90915"), results[0].Model.MerchantID);
        Assert.Equal(39400M, results[0].Model.Amount);
        Assert.Equal(CurrencyTypeEnum.EUR, results[0].Model.Currency);
        Assert.Equal("5001834", results[0].Model.OrderID);
        Assert.Equal(PaymentMethodTypeEnum.pisp, results[0].Model.PaymentMethodTypes);
        Assert.Equal("RANGE ROVER TDV8", results[0].Model.Description);
        Assert.Equal("Jane", results[0].Model.ShippingFirstName);
        Assert.Equal("Doe", results[0].Model.ShippingLastName);
        Assert.Equal("blackhole@nofrixion.com", results[0].Model.CustomerEmailAddress);
        Assert.Equal(PartialPaymentMethodsEnum.Partial, results[0].Model.PartialPaymentMethod);
        Assert.True(results[0].Model.UseHostedPaymentPage);
        Assert.Equal("RANGE ROVER TDV8", results[0].Model.Title);
    }
}
