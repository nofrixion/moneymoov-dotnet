//-----------------------------------------------------------------------------
// Filename: BeneficiaryValidationTests.cs
//
// Description: Unit tests for the validation of the MoneyMoov Beneficiary models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 Feb 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
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

public class BeneficairyValidationTests : MoneyMoovUnitTestBase<BeneficairyValidationTests>
{
    public BeneficairyValidationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a valid beneficiary model does not return any validation errors.
    /// </summary>
    [Fact]
    public void Beneficiary_Valid_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiary = new Beneficiary
        {
            ID = Guid.NewGuid(),
            MerchantID = Guid.NewGuid(),
            Name = "Test",
            Currency = CurrencyTypeEnum.EUR,
            Destination = new Counterparty
            { 
                Name = "Test Dest",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907",
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        var problem = beneficiary.Validate();

        Assert.NotNull(problem);
        Assert.True(problem.IsEmpty);
        Assert.Empty(problem.Errors);
    }

    /// <summary>
    /// Tests that an empty beneficiary model returns the expected number of validation errors.
    /// </summary>
    [Fact]
    public void Beneficiary_Empty_Validation_Fails()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiary = new Beneficiary();
   
        var problem = beneficiary.Validate();

        Assert.NotNull(problem);

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(err.ToJsonFormatted());
        }

        Assert.NotEmpty(problem.Errors);
        Assert.Equal(2, problem.Errors.Count);
    }
}
