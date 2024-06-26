//-----------------------------------------------------------------------------
// Filename: BooleanAsStringConverterTests.cs
//
// Description: Unit tests for the MoneyMoov Json boolean converter class.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 10 Jun 2024  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class BooleanAsStringConverterTests
{
    private class TestClass
    {
        public bool MyBoolean { get; set; }
    }

    [Theory]
    [InlineData("\"true\"", true)]
    [InlineData("\"false\"", false)]
    [InlineData("\"True\"", true)]
    [InlineData("\"False\"", false)]
    [InlineData("\"TRUE\"", true)]
    [InlineData("\"FALSE\"", false)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("1", true)]
    [InlineData("123", true)]
    [InlineData("0", false)]
    public void Deserialize_Bool_Success(string json, bool expected)
    {
        var result = json.FromJson<bool>();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void Serialize_Bool_Success(bool value, string expected)
    {
        var json = value.ToJsonFlat();
        Assert.Equal(expected, json);
    }

    [Fact]
    public void Deserialize_Null_Bool_Failure()
    {
        Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => "null".FromJson<bool>());
    }

    [Theory]
    [InlineData("\"notabool\"")]
    [InlineData("\"123\"")]
    public void Deserialize_Bool_Failure(string json)
    {
        Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => json.FromJson<bool>());
    }

    [Theory]
    [InlineData("{\"MyBoolean\":\"true\"}", true)]
    [InlineData("{\"MyBoolean\":\"false\"}", false)]
    public void Deserialize_ClassWithBoolProperty_Success(string json, bool expected)
    {
        var result = json.FromJson<TestClass>();
        Assert.NotNull(result);
        Assert.Equal(expected, result.MyBoolean);
    }

    [Theory]
    [InlineData(true, "{\"myBoolean\":true}")]
    [InlineData(false, "{\"myBoolean\":false}")]
    public void Serialize_ClassWithBoolProperty_Success(bool value, string expected)
    {
        var myClass = new TestClass { MyBoolean = value };
        var json = myClass.ToJsonFlat();
        Assert.Equal(expected, json);
    }
}