// -----------------------------------------------------------------------------
//  Filename: HmacSignatureAuthHelperTests.cs
// 
//  Description: HmacSignatureAuthHelper Tests:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  16 01 2023  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
//  20 11 2024  Aaron Clauson    Moved from NoFrixion.Core to MoneyMoov assembly.  
//
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class HmacSignatureAuthHelperTests
{
    private readonly ILogger<HmacSignatureAuthHelperTests> _logger;
    private LoggerFactory _loggerFactory;

    public HmacSignatureAuthHelperTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<HmacSignatureAuthHelperTests>();
    }

    [Fact]
    public void GenerateSignature_Test()
    {
        DateTime date = DateTime.Parse("Fri, 13 Jan 2023 12:21:17 GMT");
        var existingSignature = "qsL1e%2FVbz92f%2FUj3zqEg3bboI%2FA%3D";

        var secret = "123456Q3MDc2YjNhNDUzMTk1NzljZmVj";
        var nonce = "abdc0084-9a07-463b-ba8c-f47f2285531b";

        var signatureLegacy = HmacSignatureBuilder.GenerateSignature(nonce, date, secret, 0, SharedSecretAlgorithmsEnum.HMAC_SHA1);

        Assert.Equal(existingSignature, signatureLegacy);

        _logger.LogInformation($"Signature: {signatureLegacy}");
    }

    /// <summary>
    /// Tests that the HMAC generation can cope with a short secret.
    /// </summary>
    [Fact]
    public void Generate_Signature_Short_Password_Test()
    {
        DateTime date = DateTime.Parse("Fri, 13 Jan 2023 12:21:17 GMT");

        var secret = "password";
        var nonce = "abdc0084-9a07-463b-ba8c-f47f2285531b";

        var signature = HmacSignatureBuilder.GenerateSignature(nonce, date, secret, 1, SharedSecretAlgorithmsEnum.HMAC_SHA256);

        Assert.NotNull(signature);

        _logger.LogInformation($"Signature: {signature}");
    }

    /// <summary>
    /// Tests that a version 1 signature generates the expected signature.
    /// </summary>
    [Fact]
    public void GenerateSignature_Version1_Success()
    {
        DateTime date = DateTime.Parse("Wed, 20 Nov 2024 12:21:17 GMT");
        var existingSignature = "rWnfYK2BqiWrF2xGYdShgUhvFJO8gBWWnHLHKIV1X2KoUalr3Xf1MZ2oxs9%2BKQGyONVcSyVv3th7afcFPh7SPw%3D%3D";

        var secret = "123456Q3MDc2YjNhNDUzMTk1NzljZmVj";
        var nonce = "abdc0084-9a07-463b-ba8c-f47f2285531b";

        var signatureV1 = HmacSignatureBuilder.GenerateSignature(nonce, date, secret, 1, SharedSecretAlgorithmsEnum.HMAC_SHA512);

        _logger.LogInformation($"Signature: {signatureV1}");

        Assert.Equal(existingSignature, signatureV1);
    }
}