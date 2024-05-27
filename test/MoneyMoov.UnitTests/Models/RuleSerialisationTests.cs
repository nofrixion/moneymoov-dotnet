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
using NoFrixion.Biz.BizModels.Paging;
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

    /// <summary>
    /// Tests that a list of rules in a paged response can be deserialised from a JSON representation.
    /// </summary>
    [Fact]
    public void Deserialise_SystemJson_Sweep_Rules_List_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var rulesJson = @"
{
  ""content"": [
    {
      ""id"": ""e409ec3a-5a05-404f-0768-08dc7b17a061"",
      ""accountID"": ""fe37ca97-be54-4a8a-9745-07667adcd9ba"",
      ""merchantID"": ""ae1d90bb-6c1a-477b-a2cd-bc3073b3146f"",
      ""userID"": ""a284a9ac-1c2d-454b-a973-d75a6d497051"",
      ""name"": ""Get_Merchant_Rules_Success"",
      ""isDisabled"": false,
      ""status"": ""PendingApproval"",
      ""triggerOnPayIn"": false,
      ""triggerOnPayOut"": false,
      ""sweepAction"": {
        ""priority"": 0,
        ""actionType"": ""Sweep"",
        ""destinations"": [
          {
            ""sweepPercentage"": 97,
            ""sweepAmount"": 0,
            ""priority"": 0,
            ""isDisabled"": false,
            ""name"": ""Jane Doe"",
            ""identifier"": {
              ""type"": ""IBAN"",
              ""currency"": ""EUR"",
              ""iban"": ""IEMOCK123456779"",
              ""summary"": ""IBAN: IEMOCK123456779"",
              ""displaySummary"": ""IEMOCK123456779"",
              ""displayScanSummary"": ""No identifier.""
            },
            ""summary"": ""Jane Doe, IBAN: IEMOCK123456779""
          }
        ],
        ""amountToLeave"": 1,
        ""minimumAmountToRunAt"": 0
      },
      ""approveUrl"": ""identity-dev.nofrixion.com/connect/authorize?client_id=&scope=openid%20mfa%20nofrixion.strong%20ruleid%3Ae409ec3a-5a05-404f-0768-08dc7b17a061%20rulehash%3A1WK5hmi6fZHlfVrXyuoVDzMs_66na-Vrd4o8J-f0caI&response_type=code&state=e409ec3a-5a05-404f-0768-08dc7b17a061&acr_values=mfa&redirect_uri="",
      ""inserted"": ""2024-05-23T11:00:59.6471449+00:00"",
      ""lastUpdated"": ""2024-05-23T12:00:59.6479073+01:00"",
      ""lastRunAtTransactionDate"": ""0001-01-01T00:00:00+00:00"",
      ""createdBy"": {
        ""id"": ""a284a9ac-1c2d-454b-a973-d75a6d497051"",
        ""firstName"": ""Jane"",
        ""lastName"": ""Doe"",
        ""emailAddress"": ""jane.doe@nofrixion.com"",
        ""roles"": [],
        ""twoFactorEnabled"": false,
        ""passkeyAdded"": false
      },
      ""account"": {
        ""id"": ""fe37ca97-be54-4a8a-9745-07667adcd9ba"",
        ""merchantID"": ""ae1d90bb-6c1a-477b-a2cd-bc3073b3146f"",
        ""status"": 0,
        ""balance"": 1.21,
        ""submittedPayoutsBalance"": 0,
        ""pendingPayinsBalance"": 0,
        ""inserted"": ""2024-05-23T12:00:59.2806536+01:00"",
        ""lastUpdated"": ""2024-05-23T12:00:59.2806536+01:00"",
        ""currency"": ""EUR"",
        ""iban"": ""IE64IRCE92050112345678"",
        ""sortCode"": """",
        ""accountNumber"": """",
        ""bic"": """",
        ""accountName"": ""testaccount"",
        ""identifier"": {
          ""type"": ""IBAN"",
          ""currency"": ""EUR"",
          ""iban"": ""IE64IRCE92050112345678"",
          ""sortCode"": """",
          ""accountNumber"": """",
          ""summary"": ""IBAN: IE64IRCE92050112345678"",
          ""displaySummary"": ""IE64IRCE92050112345678"",
          ""displayScanSummary"": ""No identifier.""
        },
        ""displayName"": ""testaccount (fe37ca97-be54-4a8a-9745-07667adcd9ba)"",
        ""summary"": ""testaccount, IBAN: IE64IRCE92050112345678"",
        ""isDefault"": false,
        ""availableBalance"": 1.21,
        ""accountSupplierName"": ""Modulr"",
        ""isConnectedAccount"": false
      }
    }
  ],
  ""pageNumber"": 1,
  ""pageSize"": 20,
  ""totalPages"": 1,
  ""totalSize"": 1
}";

        var rules = System.Text.Json.JsonSerializer.Deserialize<RulesPageResponse>(rulesJson, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

        Assert.NotNull(rules);
        Assert.Single(rules?.Content);
        Assert.Equal("Get_Merchant_Rules_Success", rules?.Content[0].Name);
        Assert.Single(rules?.Content[0].SweepAction.Destinations);
        Assert.Equal("Jane Doe", rules?.Content[0].SweepAction.Destinations[0].Name);
        Assert.Equal(97M, rules?.Content[0].SweepAction.Destinations[0].SweepPercentage);
        Assert.Equal(CurrencyTypeEnum.EUR, rules?.Content[0].SweepAction.Destinations[0]?.Identifier?.Currency);
        Assert.Equal("IEMOCK123456779", rules?.Content[0].SweepAction.Destinations[0]?.Identifier?.IBAN);
        Assert.Equal(1.0M, rules?.Content[0].SweepAction.AmountToLeave);

    }
}