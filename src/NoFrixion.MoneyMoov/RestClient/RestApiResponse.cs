//-----------------------------------------------------------------------------
// Filename: RestApiResponse.cs
//
// Description: A response wrapper class to hold the attempt or result
// of calling a REST endpoint.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using LanguageExt;
using System.Net;
using System.Net.Http.Headers;

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Base class for REST API Response.
/// </summary>
public class RestApiResponse
{
    /// <summary>
    /// Gets or sets the status code (HTTP status code)
    /// </summary>
    /// <value>The status code.</value>
    public HttpStatusCode StatusCode { get; }

    public Option<Uri> RequestUri { get; }

    /// <summary>
    /// Gets or sets the HTTP headers
    /// </summary>
    /// <value>HTTP headers</value>
    public Option<HttpResponseHeaders> Headers { get; }

    public NoFrixionProblem Problem { get; } = NoFrixionProblem.Empty;

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, HttpResponseHeaders headers)
    {
        StatusCode = statusCode;
        RequestUri = requestUri ?? Option<Uri>.None;
        Headers = headers;
    }

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, NoFrixionProblem problem) :
        this(statusCode, requestUri, Option<HttpResponseHeaders>.None, problem)
    { }

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, Option<HttpResponseHeaders> headers, NoFrixionProblem problem)
    {
        StatusCode = statusCode;
        RequestUri = requestUri ?? Option<Uri>.None;
        Headers = headers;
        Problem = problem;
    }

    public string ToJson()
        => this.ToJsonFormatted();
}

/// <summary>
/// Generic REST API Response.
/// </summary>
public class RestApiResponse<T> : RestApiResponse
{
    /// <summary>
    /// Gets or sets the data (parsed HTTP body)
    /// </summary>
    /// <value>The data.</value>
    public Option<T> Data { get; } = Option<T>.None;

    /// <summary>
    /// Attempts to get the result.
    /// </summary>
    public T? Result => Data.IsNone ? default : (T)Data;

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, HttpResponseHeaders headers)
        : base(statusCode, requestUri, headers)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RestApiResponse{T}" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="requestUri">The URI to send the request to.</param>
    /// <param name="headers">HTTP headers.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, HttpResponseHeaders headers, T data)
        :  base(statusCode, requestUri, headers)
    {
        Data = data;
    }

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, NoFrixionProblem problem)
    : base(statusCode, requestUri, problem)
    { }

    public RestApiResponse(HttpStatusCode statusCode, Uri? requestUri, Option<HttpResponseHeaders> headers, NoFrixionProblem problem)
        : base(statusCode, requestUri, headers, problem)
    { }
 }

/// <summary>
/// REST API Response for file content.
/// </summary>
public class RestApiFileResponse : RestApiResponse
{
    public byte[]? FileContent { get; }
    public string? ContentType { get; }
    public string? FileName { get; }

    public RestApiFileResponse(HttpStatusCode statusCode, Uri? requestUri, HttpResponseHeaders headers, byte[] fileContent, string contentType, string fileName)
        : base(statusCode, requestUri, headers)
    {
        FileContent = fileContent;
        ContentType = contentType;
        FileName = fileName;
    }

    public RestApiFileResponse(HttpStatusCode statusCode, Uri? requestUri, NoFrixionProblem problem)
        : base(statusCode, requestUri, problem)
    { }

    public RestApiFileResponse(HttpStatusCode statusCode, Uri? requestUri, Option<HttpResponseHeaders> headers, NoFrixionProblem problem)
        : base(statusCode, requestUri, headers, problem)
    { }
}