//-----------------------------------------------------------------------------
// Filename: HttpSignatureHeaders.cs
// 
// Description: Represents the headers used in the HTTP Signature.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 20 Nov 2024  Donal O'Connor   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http.Headers;

namespace NoFrixion.MoneyMoov;

public class HttpSignatureHeaders
{
    public string? Nonce { get; set; }

    public DateTime Date { get; set; } = DateTime.MinValue;

    public string? Signature { get; set; }

    public HttpSignatureHeaders()
    { }

    public HttpSignatureHeaders(HttpRequest request)
    {
        IDictionary<string, string> httpRequestHeaders = request.Headers.ToDictionary(
                     header => header.Key,
                     header => string.Join(", ", header.Value.ToString() ?? string.Empty),
                     StringComparer.OrdinalIgnoreCase);

        var nonce = httpRequestHeaders.ContainsKey(HmacAuthenticationConstants.NONCE_HEADER_NAME) ?
            httpRequestHeaders[HmacAuthenticationConstants.NONCE_HEADER_NAME].ToString() : null;

        if (string.IsNullOrEmpty(nonce) && httpRequestHeaders.ContainsKey(HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME))
        {
            nonce = httpRequestHeaders[HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME]?.Trim();
        }

        Nonce = nonce;

        var date = DateTime.MinValue;

        // Parse the date from the header. Must be in RFC1123 date format. E.g. Fri, 01 Mar 2019 15:00:00 GMT.
        if (httpRequestHeaders.ContainsKey(HmacAuthenticationConstants.DATE_HEADER_NAME) &&
            DateTime.TryParseExact(httpRequestHeaders[HmacAuthenticationConstants.DATE_HEADER_NAME], "R", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date))
        {
            Date = date;
        }

        if (httpRequestHeaders.ContainsKey(HmacAuthenticationConstants.AUTHORIZATION_HEADER_NAME) &&
            AuthenticationHeaderValue.TryParse(httpRequestHeaders[HmacAuthenticationConstants.AUTHORIZATION_HEADER_NAME], out var authorisationHeader))
        {
            if (authorisationHeader.Scheme == HmacAuthenticationConstants.SIGNATURE_SCHEME_NAME)
            {
                // Split Authentication signature into a dictionary
                var authenticationParams = authorisationHeader.Parameter?.Split(',')
                    .Select(p => p.Split('='))
                    .ToDictionary(keyPair => keyPair[0].Trim(), keyPair => keyPair[1].Trim('"', ' '), StringComparer.OrdinalIgnoreCase);

                if (authenticationParams?.ContainsKey(HmacAuthenticationConstants.SIGNATURE_SCHEME_NAME) == true)
                {
                    Signature = authenticationParams[HmacAuthenticationConstants.SIGNATURE_SCHEME_NAME].Trim();
                }
            }
        }
    }

    /// <summary>
    /// Verifies the headers are valid for the signature version.
    /// </summary>
    /// <param name="signatureVersion">The signature version to verify the header values for. Different
    /// versions are expected to require different header values.</param>
    /// <returns>If the header values were valid an empty problem, otherwise a problem with a list of errors.</returns>
    public NoFrixionProblem Verify(int signatureVersion)
    {
        List<ValidationResult> headerProblems = new List<ValidationResult>();

        if(string.IsNullOrWhiteSpace(Nonce))
        {
            headerProblems.Add(new ValidationResult("Nonce was missing.", new [] { nameof(Nonce) }));
        }

        if (Date == DateTime.MinValue)
        {
            headerProblems.Add(new ValidationResult("Date was missing.", new[] { nameof(Date) }));
        }

        if (string.IsNullOrWhiteSpace(Signature))
        {
            headerProblems.Add(new ValidationResult("Signature was missing.", new[] { nameof(Signature) }));
        }

        if (headerProblems.Count > 0)
        {
           return new NoFrixionProblem("There were one or more validation errors with the HTTP request headers when attempting to validate the authentication signature.", headerProblems);
        }
        else
        {
            return NoFrixionProblem.Empty;
        }
    }
}
