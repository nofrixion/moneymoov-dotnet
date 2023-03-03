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
            YourReference = "Your-Ref-123",
            TheirReference = "Their-Ref-123",
            Currency = CurrencyTypeEnum.EUR,
            DestinationAccount = new Counterparty
            { 
                Name = "Test Dest",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907"
                }
            }
        };

        var problem = beneficiary.Validate();

        Assert.NotNull(problem);
        Assert.True(problem.IsEmpty);
        Assert.Empty(problem.Errors);
    }

    /// <summary>
    /// Tests that an empty beneficiary model retruns the expected number of validation errors.
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
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(err));
        }

        Assert.NotEmpty(problem.Errors);
        Assert.Equal(4, problem.Errors.Count);
    }

    /// <summary>
    /// Tests that a beneficiary with an invalid their reference fails.
    /// </summary>
    [Fact]
    public void Beneficiary_Invalid_TheirRef_Fails()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiary = new Beneficiary
        {
            ID = Guid.NewGuid(),
            MerchantID = Guid.NewGuid(),
            Name = "Test",
            YourReference = "Your-Ref-123",
            TheirReference = "XXXX",
            Currency = CurrencyTypeEnum.EUR,
            DestinationAccount = new Counterparty
            {
                Name = "Test Dest",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907"
                }
            }
        };
        var problem = beneficiary.Validate();

        Assert.NotNull(problem);

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(err));
        }

        Assert.NotEmpty(problem.Errors);
        Assert.Single(problem.Errors);
    }

    /// <summary>
    /// Tests that a beneficiary with an invalid your reference fails.
    /// </summary>
    [Fact]
    public void Beneficiary_Invalid_YourRef_Fails()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiary = new Beneficiary
        {
            ID = Guid.NewGuid(),
            MerchantID = Guid.NewGuid(),
            Name = "Test",
            YourReference = "€5",
            TheirReference = "Their-Ref-123",
            Currency = CurrencyTypeEnum.EUR,
            DestinationAccount = new Counterparty
            {
                Name = "Test Dest",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907"
                }
            }
        };
        var problem = beneficiary.Validate();

        Assert.NotNull(problem);

        foreach (var err in problem.Errors)
        {
            Logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(err));
        }

        Assert.NotEmpty(problem.Errors);
        Assert.Single(problem.Errors);
    }
}
