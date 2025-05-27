// -----------------------------------------------------------------------------
//  Filename: HttpClientConstants.cs
// 
//  Description: Constants for each kind of HttpClient:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  09 03 2022  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//  15 11 2022  Arif Matin       Added Yapily client.
//  15 08 2023  Aaron Clauson    Added Banking Circle.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Constants;

public static class HttpClientConstants
{
    public static string HTTP_SIGNATURE_CLIENT_NAME = "HttpSignatureClient";

    public static string HTTP_CHECKOUT_CLIENT_NAME = "CheckoutClient";

    public static string HTTP_NOFRIXION_IDENTITY_CLIENT_NAME = "NoFrixionIdentityClient";

    public static string HTTP_APPLE_PAY_CLIENT_NAME = "ApplePayClient";

    public static string HTTP_YAPILY_CLIENT_NAME = "YapilyClient";

    public static string HTTP_BANKINGCIRCLE_API_CLIENT_NAME = "BankingCircleApiClient";

    public static string HTTP_BANKINGCIRCLE_AUTHORISATION_CLIENT_NAME = "BankingCircleAuthorisationClient";

    public static string HTTP_BANKINGCIRCLE_DIRECTDEBIT_API_CLIENT_NAME = "BankingCircleDirectDebitApiClient";

    public static string HTTP_BANKINGCIRCLE_FX_API_CLIENT_NAME = "BankingCircleFxClient";

    /// <summary>
    /// The minimum value the timeout for the HTTP client timeout can be set to. The HTTP client
    /// timeout is for services, such as Modulr and CyberSource, that use HTTP clients to
    /// connect to an external HTTP server.
    /// </summary>
    public const int HTTP_CLIENT_TIMEOUT_SECONDS_MINIMUM = 2;

    /// <summary>
    /// The default value the timeout for the HTTP client timeout will be set to. The HTTP client
    /// timeout is for services, such as Modulr and CyberSource, that use HTTP clients to
    /// connect to an external HTTP server.
    /// </summary>
    public const int HTTP_CLIENT_TIMEOUT_SECONDS_DEFAULT = 10;

    /// <summary>
    /// The timeout the HTTP client timeout will be set to one minute. The HTTP client
    /// timeout is for services, such as Yapily, that use HTTP clients to
    /// connect to an external HTTP server. Yapily requests for AIS can take up
    /// to a minute for responses.
    /// </summary>
    public const int HTTP_CLIENT_TIMEOUT_SECONDS_ONE_MINUTE = 60;
}