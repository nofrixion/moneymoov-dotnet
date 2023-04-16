// -----------------------------------------------------------------------------
// Filename: NoFrixionProblem.cs
// 
// Description: Based on the ASP.NET Core ProblemDetails class which is in turn
// based on  https://tools.ietf.org/html/rfc7807. It provides a standard way to
// deliver API error information to callers.
//
// References:
// Unfortunately the below class is in the ASP.Net Core MVC assembly which is not
// practical to reference from non MVC Web applications.
// https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 23 Jan 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Taken from ASP.NET Core MVC ProblemDetailsDefaults class. Using the same values for consistency.
/// </summary>
public static class ProblemDefaults
{
    public static readonly Dictionary<int, (string Type, string Title)> Defaults = new()
    {
        [400] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            "Bad Request"
        ),

        [401] =
        (
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            "Unauthorized"
        ),

        [403] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            "Forbidden"
        ),

        [404] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            "Not Found"
        ),

        [406] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.6",
            "Not Acceptable"
        ),

        [409] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            "Conflict"
        ),

        // This response code is being used to indicate an operation, such as sending an HTTP request,
        // did not occur as there was a problem before it could be attempted.
        [412] =
        (
            "https://tools.ietf.org/html/rfc7232#section-4.2",
            "Precondition Failed"
        ),

        [415] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.13",
            "Unsupported Media Type"
        ),

        [422] =
        (
            "https://tools.ietf.org/html/rfc4918#section-11.2",
            "Unprocessable Entity"
        ),

        [500] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            "An error occurred while processing your request."
        ),
    };
}

public class NoFrixionUnauthorizedProblem : NoFrixionProblem
{
    private const int STATUS_CODE = 401;

    public NoFrixionUnauthorizedProblem(string errorMessage) : base(errorMessage, STATUS_CODE)
    {
    }
}

public class NoFrixionForbiddenProblem : NoFrixionProblem
{
    private const int STATUS_CODE = 403;

    public NoFrixionForbiddenProblem(string errorMessage) : base(errorMessage, STATUS_CODE)
    {
    }
}

public class NoFrixionNotFoundProblem : NoFrixionProblem
{
    private const int STATUS_CODE = 404;

    public NoFrixionNotFoundProblem(string errorMessage) : base(errorMessage, STATUS_CODE)
    {
    }
}

public class NoFrixionProblem
{
    private static NoFrixionProblem _empty = new NoFrixionProblem { _isEmpty = true };
    public static NoFrixionProblem Empty
    {
        get
        {
            return _empty;
        }
    }
    private bool _isEmpty = false;

    /// <summary>
    /// A URI reference [RFC3986] that identifies the problem type. This specification
    ///  encourages that, when dereferenced, it provide human-readable documentation for
    ///  the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member
    ///  is not present, its value is assumed to be "about:blank".
    /// </summary>  
    /// <example>
    /// https://tools.ietf.org/html/rfc7231#section-6.5.1
    /// </example>
    [JsonPropertyName("type")]
    public string Type { get; set; } = ProblemDefaults.Defaults[(int)HttpStatusCode.BadRequest].Type;

    /// <summary>
    /// A short, human-readable summary of the problem type.It SHOULD NOT change from
    /// occurrence to occurrence of the problem, except for purposes of localization (e.g.,
    /// using proactive content negotiation; see[RFC7231], Section 3.4).
    /// </summary>
    /// <example>
    /// Bad Request
    /// </example>
    [JsonPropertyName("title")]
    public string Title { get; set; } = ProblemDefaults.Defaults[(int)HttpStatusCode.BadRequest].Title;

    /// <summary>
    /// The HTTP status code([RFC7231], Section 6) generated by the origin server for
    /// this occurrence of the problem. 
    /// </summary>
    /// <example>
    /// 400 for Bad Request (generic).
    /// 401 for Unauthorised.
    /// 404 Not Found.
    /// 500 Internal Server Error.
    /// 501 Not Implemented.
    /// etc.
    /// </example>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    /// <example>
    /// Something went wrong with your request and this is a useful explanation of what happened.
    /// </example>
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// A URI reference that identifies the specific occurrence of the problem.It may
    /// or may not yield further information if dereferenced.
    /// </summary>
    [JsonPropertyName("instance")]
    public string Instance { get; set; } = string.Empty;

    [JsonPropertyName("traceid")]
    public string TraceID { get; set; } = string.Empty;

    /// <summary>
    /// Optional validation error messages. These are 
    /// intended to provide an additional error message about a specific field.
    /// Note that the approach to recording validation errors has been deliberately
    /// chosen to result in the same serialised JSON as will be returned by ASP.NET 
    /// when the parameter on a controller action fails validation.
    /// </summary>
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

    [JsonIgnore]
    [IgnoreDataMember]
    public bool IsEmpty => _isEmpty;

    public NoFrixionProblem()
    { }

    /// <summary>
    /// Create a new problem instance.
    /// </summary>
    /// <param name="status">The closest matching HTTP status code for the problem.</param>
    /// <param name="errorDetail">A succinct description of the error.</param>
    public NoFrixionProblem(HttpStatusCode status, string errorDetail)
    {
        Status = (int)status;

        if (ProblemDefaults.Defaults.ContainsKey((int)status))
        {
            Title = ProblemDefaults.Defaults[(int)status].Title;
            Type = ProblemDefaults.Defaults[(int)status].Type;
        }

        Detail = errorDetail;
    }

    public NoFrixionProblem(string errorMessage, int errorCode = 400)
    {
        Detail = errorMessage;
        Status = errorCode;
    }

    public NoFrixionProblem(string errorMessage, ValidationResult validationError, int errorCode = 400)
    {
        Detail = errorMessage;
        Status= errorCode;

        foreach (var member in validationError.MemberNames)
        {
            AddValidationError(member, validationError.ErrorMessage ?? string.Empty);
        }
    }

    public NoFrixionProblem(string errorMessage, IEnumerable<ValidationResult> validationErrors, int errorCode = 400)
    {
        Detail = errorMessage;
        Status = errorCode;

        if (validationErrors != null && validationErrors.Count() > 0)
        {
            foreach (var err in validationErrors)
            {
                foreach (var member in err.MemberNames)
                {
                    AddValidationError(member, err.ErrorMessage ?? string.Empty);
                }
            }
        }
    }

    public NoFrixionProblem SetTraceID(string traceID)
    {
        TraceID= traceID;
        return this;
    }

    public void AddValidationError(string propertyName, string error)
    {
        if (Errors.ContainsKey(propertyName))
        {
            Errors[propertyName] = Errors[propertyName].Append(error).ToArray();
        }
        else
        {
            Errors.Add(propertyName, new string[] { error });
        }
    }

    public string ToJson() =>
        System.Text.Json.JsonSerializer.Serialize(this);

    /// <summary>
    /// Plain text representation of the problem.
    /// </summary>
    public string ToTextErrorMessage()
    {
        string error = string.Empty;

        if (Status != 0)
        {
            error = $"Error Status Code {Status}";
        }

        if (!string.IsNullOrEmpty(Title))
        {
            error += ": " + Title;
        }

        if (!string.IsNullOrEmpty(Detail))
        {
            error += ". " + Detail;
        }

        if (Errors.Count > 0)
        {
            error += " Validation errors: ";
            foreach (var kvp in Errors)
            {
                error += kvp.Key + ": " + string.Join(',', kvp.Value);
            }
        }

        if (!string.IsNullOrEmpty(error))
        {
            error = error.TrimEnd('.') + ".";
        }

        return error;
    }

    /// <summary>
    /// HTML representation of the problem.
    /// </summary>
    public string ToHtmlErrorMessage()
    {
        string htmlError = string.Empty;

        if (!string.IsNullOrEmpty(Title))
        {
            htmlError = $"<p><strong>{Title}</strong>: {Detail}</p>";
        }

        if (Errors.Count > 0)
        {
            htmlError += "<ul>";
            foreach (var kvp in Errors)
            {
                htmlError += $"<li>{kvp.Key}: {string.Join(',', kvp.Value)}</li>";
            }
            htmlError += "</ul>";
        }

        return htmlError;
    }

    public static NoFrixionProblem FromJson(string json)
    {
       return Newtonsoft.Json.JsonConvert.DeserializeObject<NoFrixionProblem>(json,
            new Newtonsoft.Json.JsonSerializerSettings
            {
                ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace
            }) 
            ?? NoFrixionProblem.Empty;
    }
}