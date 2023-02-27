//-----------------------------------------------------------------------------
// Filename: RuleValidationTests.cs
//
// Description: Unit tests for the validation of the MoneyMoov Rule models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 02 Feb 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
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

public class RuleValidationTests : MoneyMoovUnitTestBase<RuleValidationTests>
{
    public RuleValidationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a sweep rule generates a validation error if the currency is not set.
    /// </summary>
    [Fact]
    public void Rule_No_Identifier_Currency_Validation_Fails()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var rule = new Rule
        {
            SweepAction = new SweepAction
            {
                Priority = 1,
                AmountToLeave = 1.00M,
                MinimumAmountToRunAt = 99.00M,
                ActionType = RuleActionsEnum.Sweep,
                Destinations = new List<SweepDestination>
                {
                    new SweepDestination
                    {
                        SweepPercentage = 100,
                        Name = "Jane Doe",
                        Identifier = new AccountIdentifier
                        {
                            IBAN = "IEMOCK123456779"
                        }
                    }
                }
            }
        };

        var problem = rule.Validate();

        Assert.NotNull(problem);

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(err));
        }

        Assert.NotEmpty(problem.Errors);
    }

    [Theory]
    [InlineData("0 0/15 * * * ?", false)] // Will fail as the new value is less than 60 minutes.
    [InlineData("0 0/10 * * * ?", false)] // Will fail as the new value is less than 60 minutes.
    [InlineData("0 0 * ? * *", true)] // Will succeed as the expressions runs every hour.
    [InlineData("0 0 */2 ? * *", true)] //Will succeed as the expressions runs every two hour.
    [InlineData("0 0 0/1 ? * * *", true)] //Will succeed as the expressions runs every hour.
    [InlineData("0 0 0 * * ?", true)] //Will succeed as the expressions runs every day at midnight - 12am.
    [InlineData("0 0 8 ? * MON-FRI *", true)] // Cron expression is every weekday at 8am. Will succeed as the new value is greater or equal than 60 minutes.
    public void Rule_ValidateCronExpression(string cronExpression, bool shouldSucceed)
    {

        var rule = new Rule
        {
            TriggerCronExpression = cronExpression,
            SweepAction = new SweepAction
            {
                Priority = 1,
                AmountToLeave = 1.00M,
                MinimumAmountToRunAt = 99.00M,
                ActionType = RuleActionsEnum.Sweep,
                Destinations = new List<SweepDestination>
                {
                    new SweepDestination
                    {
                        SweepPercentage = 100,
                        Name = "Jane Doe",
                        Identifier = new AccountIdentifier
                        {
                            Currency = "EUR",
                            IBAN = "IEMOCK123456779"
                        }
                    }
                }
            }
        };

        var problem = rule.Validate();

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(err));
        }

        Assert.Equal(problem.Errors.Count == 0, shouldSucceed);

    }
}
