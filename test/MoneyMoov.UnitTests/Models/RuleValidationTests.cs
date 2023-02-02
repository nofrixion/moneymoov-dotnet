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
}