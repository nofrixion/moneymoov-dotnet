// -----------------------------------------------------------------------------
// Filename: NoFrixionProblem.cs
// 
// Description: This class is used to return an error result from a mediatr
// handler back to the consuming controller. It's intended to be used where
// a useful error message can be returned to the API caller without having
// to resort to throwing an exception.
//
// This class should be used in conjunction with exceptions, rather than 
// a complete replacement. As a general rule to thumb this class should be
// used to convey business level errors. Any deeper issues, such as  
// connection failure to the database, still warrant an exception.
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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov;

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
    public static NoFrixionProblem Empty = new NoFrixionProblem(string.Empty);

    /// <summary>
    /// The main error message for the problem.
    /// </summary>
    public string ErrorMessage { get; init; }

    /// <summary>
    /// The error code for the problem.
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// Optional validation error messages. These are 
    /// intended to provide an additional error message about a specific field.
    /// Note that the approach to recording validation errors has been deliberately
    /// taken to result in the same serialised JSON as will be returned by ASP.NET 
    /// when the parameter on a controller action fails validation.
    /// </summary>
    public NoFrixionValidationProblemDetails ValidationErrors { get; }

    public bool IsEmpty() =>
        ErrorMessage == string.Empty;

    public NoFrixionProblem() : this(string.Empty)
    { }

    public NoFrixionProblem(string errorMessage, int errorCode = 400)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        ValidationErrors = new NoFrixionValidationProblemDetails();
    }

    public NoFrixionProblem(string errorMessage, ValidationResult validationError, int errorCode = 400)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        ValidationErrors = new NoFrixionValidationProblemDetails();

        foreach (var member in validationError.MemberNames)
        {
            AddValidationError(member, validationError.ErrorMessage ?? string.Empty);
        }
    }

    public NoFrixionProblem(string errorMessage, IEnumerable<ValidationResult> validationErrors, int errorCode = 400)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        ValidationErrors = new NoFrixionValidationProblemDetails();

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

    public void AddValidationError(string propertyName, string error)
    {
        if (ValidationErrors.Errors.ContainsKey(propertyName))
        {
            ValidationErrors.Errors[propertyName] = ValidationErrors.Errors[propertyName].Append(error).ToArray();
        }
        else
        {
            ValidationErrors.Errors.Add(propertyName, new string[] { error });
        }
    }
}

public class NoFrixionValidationProblemDetails
{
    public Dictionary<string, string[]> Errors = new Dictionary<string, string[]>();

}
