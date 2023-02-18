//-----------------------------------------------------------------------------
// Filename: NoFrixionProblemTests.cs
//
// Description: Unit tests for the MoneyMoov NoFrixionProblem model.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 18 Feb 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class NoFrixionProblemTests : MoneyMoovUnitTestBase<NoFrixionProblemTests>
{
    public NoFrixionProblemTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a problem with only the detail field set can be serialised correctly.
    /// </summary>
    [Fact]
    public void Serialise_Detail_Only_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string error = "Some problem";

        var prob = new NoFrixionProblem(error);
      
        string jsonProb = prob.ToJson();

        Logger.LogDebug(prob.ToJson());

        var desrilaisedProb = NoFrixionProblem.FromJson(jsonProb);

        Assert.NotNull(desrilaisedProb);
        Assert.Equal(error, desrilaisedProb!.Detail);
    }

    /// <summary>
    /// Tests that a problem with the detail field and validation errors set can be serialised correctly.
    /// </summary>
    [Fact]
    public void Serialise_Detail_And_Validation_Errors_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string error = "Some problem";
        string validation1Field = "name";
        string validation1Error = "bad name";
        string validation2Field = "dob";
        string validation2Error = "too old";

        var prob = new NoFrixionProblem(error);
        prob.AddValidationError(validation1Field, validation1Error);
        prob.AddValidationError(validation2Field, validation2Error);

        string jsonProb = prob.ToJson();

        Logger.LogDebug(prob.ToJson());

        var desrilaisedProb = NoFrixionProblem.FromJson(jsonProb);

        Assert.NotNull(desrilaisedProb);
        Assert.Equal(error, desrilaisedProb!.Detail);
        Assert.Equal(2, desrilaisedProb.Errors.Count);
    }
}