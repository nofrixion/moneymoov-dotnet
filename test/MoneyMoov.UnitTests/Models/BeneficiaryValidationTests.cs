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
    /// Tests that changing TheirReference on a beneficiary changes the approval hash.
    /// </summary>
    [Fact]
    public void Beneficiary_GetApprovalHash_TheirReference_ChangesHash()
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

        var hashBefore = beneficiary.GetApprovalHash();

        beneficiary.TheirReference = "NEWREF";

        var hashAfter = beneficiary.GetApprovalHash();

        Assert.NotEqual(hashBefore, hashAfter);
    }

    /// <summary>
    /// Tests that changing address fields on a beneficiary's destination changes the approval hash.
    /// </summary>
    [Fact]
    public void Beneficiary_GetApprovalHash_AddressFields_ChangesHash()
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

        var hashBefore = beneficiary.GetApprovalHash();

        beneficiary.Destination.AddressLine1 = "123 Test Street";
        var hashAfterLine1 = beneficiary.GetApprovalHash();
        Assert.NotEqual(hashBefore, hashAfterLine1);

        beneficiary.Destination.AddressLine2 = "Apartment 4";
        var hashAfterLine2 = beneficiary.GetApprovalHash();
        Assert.NotEqual(hashAfterLine1, hashAfterLine2);

        beneficiary.Destination.PostTown = "Dublin";
        var hashAfterPostTown = beneficiary.GetApprovalHash();
        Assert.NotEqual(hashAfterLine2, hashAfterPostTown);

        beneficiary.Destination.PostCode = "D01 AB12";
        var hashAfterPostCode = beneficiary.GetApprovalHash();
        Assert.NotEqual(hashAfterPostTown, hashAfterPostCode);
    }

    /// <summary>
    /// Tests that two identical beneficiaries produce the same approval hash.
    /// </summary>
    [Fact]
    public void Beneficiary_GetApprovalHash_SameData_SameHash()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantId = Guid.NewGuid();

        var beneficiary1 = new Beneficiary
        {
            ID = Guid.NewGuid(),
            MerchantID = merchantId,
            Name = "Test",
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "REF001",
            Destination = new Counterparty
            {
                Name = "Test Dest",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Suite 1",
                PostTown = "London",
                PostCode = "SW1A 1AA",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907",
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        var beneficiary2 = new Beneficiary
        {
            ID = Guid.NewGuid(),
            MerchantID = merchantId,
            Name = "Test",
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "REF001",
            Destination = new Counterparty
            {
                Name = "Test Dest",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Suite 1",
                PostTown = "London",
                PostCode = "SW1A 1AA",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB42MOCK00000070629907",
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        Assert.Equal(beneficiary1.GetApprovalHash(), beneficiary2.GetApprovalHash());
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
