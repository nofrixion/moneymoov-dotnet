//-----------------------------------------------------------------------------
// Filename: WebhookTests.cs
//
// Description: Unit tests for the serialisation and deserialisation of the
// MoneyMoov Webhook models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 11 May 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class WebhookTests : MoneyMoovUnitTestBase<RuleSerialisationTests>
{
    public WebhookTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a signature for a webhook is generated as expected.
    /// </summary>
    [Fact]
    public void Webhook_Get_Signature_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string webhookPayload = "\"ID\":\"c1603a27-9acd-44c3-5286-08db4d3aa332\",\"AccountID\":\"01eabfa4-c3fd-4b2c-9c28-2e85a72d7fae\",\"Type\":\"SEPA_INST\",\"Amount\":-5.00000000000,\"Currency\":\"EUR\",\"Description\":\"Xero Invoice.\",\"TransactionDate\":\"2023-05-11T16:39:57.549+00:00\",\"Inserted\":\"2023-05-11T16:39:58.0156137+00:00\",\"YourReference\":\"xero-b58ea94a-bbc3-4df0-be0a-65f50629b104\",\"TheirReference\":\"From XXX\",\"Counterparty\":{\"Name\":\"XXX\",\"Identifier\":{\"Type\":\"IBAN\",\"IBAN\":\"23423423\",\"Summary\":\"IBAN: 23423423\"},\"Summary\":\"XXX, IBAN: 23423423\"},\"CounterpartySummary\":\"XXX, IBAN: IE123123\",\"Balance\":1757.59000000000}";

        string hmac = Webhook.GetSignature("a6565049-f9b3-4de3-8b78-03430c4f", Encoding.UTF8.GetBytes(webhookPayload));

        Logger.LogDebug("hmac=" + hmac);
    }
}