//-----------------------------------------------------------------------------
// Filename: BeneficiarySerialisationTests.cs
//
// Description: Unit tests for the serialisation and deserialisation of the
// MoneyMoov Beneficiary models.
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
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class BeneficiarySerialisationTests : MoneyMoovUnitTestBase<BeneficiarySerialisationTests>
{
    public BeneficiarySerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a beneficiary can be deserialised from its JSON represnetation
    /// using Newtonsoft.
    /// </summary>
    [Fact]
    public void Deserialise_Newtonsoft_Beneficiary_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiaryJson = @"
            {
                ""id"":""1e92214c-1650-49c4-760c-08db1746a020"",
                ""merchantID"":""2186d737-50a1-48b0-a7e7-7f39cb40407e"",
                ""name"":""Test Beneficiary"",
                ""yourReference"":""YourRef"",
                ""theirReference"":""TheirRef"",
                ""destinationAccountName"":""Destination account"",
                ""currency"":""EUR"",
                ""identifier"":{
                    ""type"":""IBAN"",
                    ""iban"":""GB33BUKB20201555555555"",
                    ""summary"":""IBAN: GB33BUKB20201555555555""
                }   
            }";

        var beneficiary = Newtonsoft.Json.JsonConvert.DeserializeObject<Beneficiary>(beneficiaryJson);

        //var rule = Newtonsoft.Json.JsonConvert.DeserializeObject<Rule>(sweepRuleJson, settings: new JsonSerializerSettings
        //{
        //    ObjectCreationHandling = ObjectCreationHandling.Replace,
        //});

        //var options = new JsonSerializerOptions();
        //options.Converters.Add(new JsonStringEnumConverter());
        //var rule = await response.Content.ReadFromJsonAsync<Rule>(options);

        Assert.NotNull(beneficiary);
        Assert.NotNull(beneficiary.Identifier);
        Assert.Equal("1e92214c-1650-49c4-760c-08db1746a020", beneficiary.ID.ToString());
        Assert.Equal("2186d737-50a1-48b0-a7e7-7f39cb40407e", beneficiary.MerchantID.ToString());
        Assert.Equal("Test Beneficiary", beneficiary.Name);
        Assert.Equal("YourRef", beneficiary.YourReference);
        Assert.Equal("TheirRef", beneficiary.TheirReference);
        Assert.Equal("Destination account", beneficiary.DestinationAccountName);
        Assert.Equal(AccountIdentifierType.IBAN, beneficiary.Identifier.Type);
        Assert.Equal("GB33BUKB20201555555555", beneficiary.Identifier.IBAN);
    }

    /// <summary>
    /// Tests that a beneficiary can be deserialised from its JSON represnetation
    /// using System.Text.
    /// </summary>
    [Fact]
    public void Deserialise_System_Text_Beneficiary_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var beneficiaryJson = @"
            {
                ""id"":""1e92214c-1650-49c4-760c-08db1746a020"",
                ""merchantID"":""2186d737-50a1-48b0-a7e7-7f39cb40407e"",
                ""name"":""Test Beneficiary"",
                ""yourReference"":""YourRef"",
                ""theirReference"":""TheirRef"",
                ""destinationAccountName"":""Destination account"",
                ""currency"":""EUR"",
                ""identifier"":{
                    ""type"":""IBAN"",
                    ""iban"":""GB33BUKB20201555555555"",
                    ""summary"":""IBAN: GB33BUKB20201555555555""
                }   
            }";

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter());
        var beneficiary = System.Text.Json.JsonSerializer.Deserialize<Beneficiary>(beneficiaryJson, options);
        
        Assert.NotNull(beneficiary);
        Assert.NotNull(beneficiary.Identifier);
        Assert.Equal("1e92214c-1650-49c4-760c-08db1746a020", beneficiary.ID.ToString());
        Assert.Equal("2186d737-50a1-48b0-a7e7-7f39cb40407e", beneficiary.MerchantID.ToString());
        Assert.Equal("Test Beneficiary", beneficiary.Name);
        Assert.Equal("YourRef", beneficiary.YourReference);
        Assert.Equal("TheirRef", beneficiary.TheirReference);
        Assert.Equal("Destination account", beneficiary.DestinationAccountName);
        Assert.Equal(AccountIdentifierType.IBAN, beneficiary.Identifier.Type);
        Assert.Equal("GB33BUKB20201555555555", beneficiary.Identifier.IBAN);
    }

}