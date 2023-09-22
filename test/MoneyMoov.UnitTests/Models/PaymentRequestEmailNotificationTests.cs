//-----------------------------------------------------------------------------
// Filename: PaymentRequestEmailNotificationTests .cs
//
// Description: Unit tests for the PaymentRequestEmailNotification model.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 20 Sep 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PaymentRequestEmailNotificationTests : MoneyMoovUnitTestBase<PaymentRequestEmailNotificationTests>
{
    public PaymentRequestEmailNotificationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a payment request values can be substituted.
    /// </summary>
    [Fact]
    public void Substitute_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var emailNotification = new PaymentRequestEmailNotification
        {
            TemplateName = "some-template",
            ToEmailAddress = "{CustomerEmailAddress}",
            FromEmailAddress = "support@nofrixion.com",
            Subject = "{Title}",
            TemplateVariables = new Dictionary<string, string>
            {
                { "CustomerName", "{CustomerName}"},
                { "OrderID", "{OrderID}" },
				{ "DisplayAmount", "{FormattedAmount}" },
				{ "Title", "{Title}"},
				{ "Description", "{Title}, {Description}"},
				{ "HostedPayCheckoutUrl", "{HostedPayCheckoutUrl}" }
            }
        };

        var paymentRequest = new PaymentRequest
        {
            ID = Guid.NewGuid(),
            Currency = CurrencyTypeEnum.EUR,
            PaymentMethodTypes = PaymentMethodTypeEnum.card | PaymentMethodTypeEnum.pisp,
            CustomerEmailAddress = "jane.doe@nofrixion.com",
            Amount = 10001.99M,
            Title = "Super Title",
            Description = "Blah blah blah.",
            HostedPayCheckoutUrl = "https://api-sandbox.nofrixion.com/pay/v2/xxx",
            OrderID = "1234",
            Addresses = new List<PaymentRequestAddress>
            {
                new PaymentRequestAddress
                {
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            }
        };

        var substitutedNotification = emailNotification.SubstituteVariables(paymentRequest, new Dictionary<string, object>());

        Assert.NotNull(substitutedNotification);
        Assert.Equal("some-template", substitutedNotification.TemplateName);
        Assert.Equal("jane.doe@nofrixion.com", substitutedNotification.ToEmailAddress);
        Assert.Equal("support@nofrixion.com", substitutedNotification.FromEmailAddress);
        Assert.Equal("Super Title", substitutedNotification.Subject);
        Assert.Equal("€10,001.99", substitutedNotification.TemplateVariables["DisplayAmount"]);
        Assert.Equal("1234", substitutedNotification.TemplateVariables["OrderID"]);
        Assert.Equal("Jane Doe", substitutedNotification.TemplateVariables["CustomerName"]);
        Assert.Equal("Super Title", substitutedNotification.TemplateVariables["Title"]);
        Assert.Equal("Super Title, Blah blah blah.", substitutedNotification.TemplateVariables["Description"]);
        Assert.Equal("https://api-sandbox.nofrixion.com/pay/v2/xxx", substitutedNotification.TemplateVariables["HostedPayCheckoutUrl"]);
    }

    /// <summary>
    /// Tests that the template string is formatted correctly when the values are from both a payment request
    /// and an additional dictionary.
    /// </summary>
    [Fact]
    public void Substitute_With_PaymentRequest_And_Dictionary_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var emailNotification = new PaymentRequestEmailNotification
        {
            TemplateName = "some-template",
            ToEmailAddress = "{CustomerEmailAddress}",
            FromEmailAddress = "support@nofrixion.com",
            Subject = "{Title}",
            TemplateVariables = new Dictionary<string, string>
            {
                { "CustomerName", "{CustomerName}"},
                { "OrderID", "{OrderID}" },
                { "DisplayAmount", "{FormattedAmount}" },
                { "Title", "{Title}"},
                { "Description", "{Title}, {Description}"},
                { "HostedPayCheckoutUrl", "{HostedPayCheckoutUrl}" },
                { "CompanyName", "{CompanyName}" }
            }
        };

        var paymentRequest = new PaymentRequest
        {
            ID = Guid.NewGuid(),
            Currency = CurrencyTypeEnum.EUR,
            PaymentMethodTypes = PaymentMethodTypeEnum.card | PaymentMethodTypeEnum.pisp,
            CustomerEmailAddress = "jane.doe@nofrixion.com",
            Amount = 10001.99M,
            Title = "Super Title",
            Description = "Blah blah blah.",
            HostedPayCheckoutUrl = "https://api-sandbox.nofrixion.com/pay/v2/xxx",
            OrderID = "1234",
            Addresses = new List<PaymentRequestAddress>
            {
                new PaymentRequestAddress
                {
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            }
        };

        var csvMapping = new Dictionary<string, object>
        {
            { "CompanyName", "Some Company" }
        };

        var substitutedNotification = emailNotification.SubstituteVariables(paymentRequest, csvMapping);

        Assert.NotNull(substitutedNotification);
        Assert.Equal("some-template", substitutedNotification.TemplateName);
        Assert.Equal("jane.doe@nofrixion.com", substitutedNotification.ToEmailAddress);
        Assert.Equal("support@nofrixion.com", substitutedNotification.FromEmailAddress);
        Assert.Equal("Super Title", substitutedNotification.Subject);
        Assert.Equal("€10,001.99", substitutedNotification.TemplateVariables["DisplayAmount"]);
        Assert.Equal("1234", substitutedNotification.TemplateVariables["OrderID"]);
        Assert.Equal("Jane Doe", substitutedNotification.TemplateVariables["CustomerName"]);
        Assert.Equal("Super Title", substitutedNotification.TemplateVariables["Title"]);
        Assert.Equal("Super Title, Blah blah blah.", substitutedNotification.TemplateVariables["Description"]);
        Assert.Equal("https://api-sandbox.nofrixion.com/pay/v2/xxx", substitutedNotification.TemplateVariables["HostedPayCheckoutUrl"]);
        Assert.Equal("Some Company", substitutedNotification.TemplateVariables["CompanyName"]);
    }
}