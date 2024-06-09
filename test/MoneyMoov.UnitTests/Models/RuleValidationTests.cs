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
using Xunit;
using Xunit.Abstractions;
using System.Text.Json;

namespace NoFrixion.MoneyMoov.UnitTests;

public class RuleValidationTests : MoneyMoovUnitTestBase<RuleValidationTests>
{
    public RuleValidationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Theory]
    [InlineData("0/15 * * * * ?", false)] // Will fail as the new value is less than 60 minutes.
    [InlineData("0/18 * * * * ?", false)] // Will fail as the new value is less than 60 minutes.
    [InlineData("0 0 * ? * *", true)] // Will succeed as the expressions runs every hour.
    [InlineData("0 0 */2 ? * *", true)] //Will succeed as the expressions runs every two hour.
    [InlineData("0 0 0/1 ? * * *", true)] //Will succeed as the expressions runs every hour.
    [InlineData("0 0 0 * * ?", true)] //Will succeed as the expressions runs every day at midnight - 12am.
    [InlineData("0 0 8 ? * MON-FRI *", true)] // Cron expression is every weekday at 8am. Will succeed as the new value is greater or equal than 60 minutes.
    [InlineData("0 0 9 2 * ?", true)] // 9am on the 2nd day of the month.
    public void Rule_ValidateCronExpression(string cronExpression, bool shouldSucceed)
    {
        var rule = new Rule
        {
            TriggerCronExpression = cronExpression,
            SweepAction = new SweepAction
            {
                AmountToLeave = 1.00M,
                MinimumAmountToRunAt = 99.00M,
                Destinations = new List<SweepDestination>
                {
                    new SweepDestination
                    {
                        SweepPercentage = 100,
                        Name = "Jane Doe",
                        Identifier = new AccountIdentifier
                        {
                            Currency = CurrencyTypeEnum.EUR,
                            IBAN = "IEMOCK123456779"
                        }
                    }
                }
            }
        };

        var problem = rule.Validate();

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(JsonSerializer.Serialize(err));
        }

        Assert.Equal(problem.Errors.Count == 0, shouldSucceed);
    }
}
