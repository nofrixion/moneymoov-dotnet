//-----------------------------------------------------------------------------
// Filename: RuleSerialisationTests.cs
//
// Description: Unit tests for the serialisation and deserialisation of the
// MoneyMoov Rule models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 Jan 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class RuleSerialisationTests : MoneyMoovUnitTestBase<RuleSerialisationTests>
{
    public RuleSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a sweep rule can be deserialised from a JSON representation.
    /// </summary>
    [Fact]
    public void Deserialise_Newtonsoft_Sweep_Rule_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var sweepRuleJson = @"
            {
              ""id"": ""60dd3c00-22bb-4a30-07f6-08daff10193e"",
              ""accountID"": ""fe37ca97-be54-4a8a-9745-07667adcd9ba"",
              ""userID"": ""8ce4c97c-ccfe-4776-8903-dae7ed1f7ca6"",
              ""name"": ""Test Sweep Rule Create"",
              ""isEnabled"": false,
              ""status"": ""PendingApproval"",
              ""triggerOnPayIn"": false,
              ""triggerOnPayOut"": false,
              ""sweepAction"": {
                ""priority"": 1,
                ""actionType"": ""Sweep"",
                ""destinations"": [
                  {
                    ""sweepPercentage"": 100,
                    ""sweepAmount"": 0,
                    ""name"": ""Jane Doe"",
                    ""identifier"": {
                      ""type"": ""IBAN"",
                      ""currency"": ""EUR"",
                      ""iban"": ""IEMOCK123456779"",
                      ""summary"": ""IBAN: IEMOCK123456779""
                    },
                    ""summary"": ""Jane Doe, IBAN: IEMOCK123456779""
                  }
                ],
                ""amountToLeave"": 1
              },
              ""inserted"": ""2023-01-25T20:09:44.1113893+00:00"",
              ""lastUpdated"": ""2023-01-25T20:09:44.1118082+00:00""
            }";

        var rule = Newtonsoft.Json.JsonConvert.DeserializeObject<Rule>(sweepRuleJson);

        Assert.NotNull(rule);
        Assert.NotNull(rule?.SweepAction);
        Assert.NotNull(rule?.SweepAction.Destinations);
        Assert.Single(rule?.SweepAction.Destinations);
        Assert.Equal(RuleStatusEnum.PendingApproval, rule?.Status);
        Assert.Null(rule?.SweepAction?.PayoutYourReference);
        Assert.Null(rule?.SweepAction?.PayoutTheirReference);
        Assert.Null(rule?.SweepAction?.PayoutDescription);
    }

    /// <summary>
    /// Tests that a sweep rule can be deserialised from a JSON representation.
    /// </summary>
    [Fact]
    public void Deserialise_SystemJson_Sweep_Rule_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var sweepRuleJson = @"
            {
              ""id"": ""60dd3c00-22bb-4a30-07f6-08daff10193e"",
              ""accountID"": ""fe37ca97-be54-4a8a-9745-07667adcd9ba"",
              ""userID"": ""8ce4c97c-ccfe-4776-8903-dae7ed1f7ca6"",
              ""name"": ""Test Sweep Rule Create"",
              ""isEnabled"": false,
              ""status"": ""PendingApproval"",
              ""triggerOnPayIn"": false,
              ""triggerOnPayOut"": false,
              ""sweepAction"": {
                ""priority"": 1,
                ""actionType"": ""Sweep"",
                ""destinations"": [
                  {
                    ""sweepPercentage"": 100,
                    ""sweepAmount"": 0,
                    ""name"": ""Jane Doe"",
                    ""identifier"": {
                      ""type"": ""IBAN"",
                      ""currency"": ""EUR"",
                      ""iban"": ""IEMOCK123456779"",
                      ""summary"": ""IBAN: IEMOCK123456779""
                    },
                    ""summary"": ""Jane Doe, IBAN: IEMOCK123456779""
                  }
                ],
                ""amountToLeave"": 1
              },
              ""inserted"": ""2023-01-25T20:09:44.1113893+00:00"",
              ""lastUpdated"": ""2023-01-25T20:09:44.1118082+00:00""
            }";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter());
        var rule = System.Text.Json.JsonSerializer.Deserialize<Rule>(sweepRuleJson, options);

        Assert.NotNull(rule);
        Assert.NotNull(rule?.SweepAction);
        Assert.NotNull(rule?.SweepAction.Destinations);
        Assert.Single(rule?.SweepAction.Destinations);
        Assert.Equal(RuleStatusEnum.PendingApproval, rule?.Status);
        Assert.Null(rule?.SweepAction?.PayoutYourReference);
        Assert.Null(rule?.SweepAction?.PayoutTheirReference);
        Assert.Null(rule?.SweepAction?.PayoutDescription);
    }

    /// <summary>
    /// Tests that a sweep rule can be deserialised from a JSON representation
    /// when the payout string patterns are set.
    /// </summary>
    [Fact]
    public void Deserialise_Sweep_Rule_With_Payout_Patterns_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string yourRefPattern = "{some_pattern} your ref";
        string theirRefPattern = "{some_pattern} their ref";
        string descriptionPattern = "Description {some_pattern}";

        var sweepRuleJson = @"
            {
              ""id"": ""60dd3c00-22bb-4a30-07f6-08daff10193e"",
              ""accountID"": ""fe37ca97-be54-4a8a-9745-07667adcd9ba"",
              ""userID"": ""8ce4c97c-ccfe-4776-8903-dae7ed1f7ca6"",
              ""name"": ""Test Sweep Rule Create"",
              ""isEnabled"": false,
              ""status"": ""PendingApproval"",
              ""triggerOnPayIn"": false,
              ""triggerOnPayOut"": false,
              ""sweepAction"": {
                ""priority"": 1,
                ""actionType"": ""Sweep"",
                ""payoutYourReference"": """ + yourRefPattern + @""",
                ""payoutTheirReference"": """ + theirRefPattern + @""",
                ""payoutDescription"": """ + descriptionPattern + @""",
                ""destinations"": [
                  {
                    ""sweepPercentage"": 100,
                    ""sweepAmount"": 0,
                    ""name"": ""Jane Doe"",
                    ""identifier"": {
                      ""type"": ""IBAN"",
                      ""currency"": ""EUR"",
                      ""iban"": ""IEMOCK123456779"",
                      ""summary"": ""IBAN: IEMOCK123456779""
                    },
                    ""summary"": ""Jane Doe, IBAN: IEMOCK123456779""
                  }
                ],
                ""amountToLeave"": 1
              },
              ""inserted"": ""2023-01-25T20:09:44.1113893+00:00"",
              ""lastUpdated"": ""2023-01-25T20:09:44.1118082+00:00""
            }";

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter());
        var rule = System.Text.Json.JsonSerializer.Deserialize<Rule>(sweepRuleJson, options);

        Assert.NotNull(rule);
        Assert.NotNull(rule?.SweepAction);
        Assert.NotNull(rule?.SweepAction.Destinations);
        Assert.Single(rule?.SweepAction.Destinations);
        Assert.Equal(RuleStatusEnum.PendingApproval, rule?.Status);
        Assert.Equal(yourRefPattern, rule?.SweepAction?.PayoutYourReference);
        Assert.Equal(theirRefPattern, rule?.SweepAction?.PayoutTheirReference);
        Assert.Equal(descriptionPattern, rule?.SweepAction?.PayoutDescription);
    }
}