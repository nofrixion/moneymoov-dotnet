// -----------------------------------------------------------------------------
//  Filename: GetResourcesResult.cs
// 
//  Description: Represents the result of a Get all query.
//  It can contain either a JSON response or a CSV export.

//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  10 Feb 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using LanguageExt;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Export;

namespace NoFrixion.MoneyMoov.Models;

public record GetResourcesResult<T> where T : class
{
    public ApiPageResponseBase<T>? PagedResourceItems { get; }
    public Option<CsvExportResponse> CsvResult { get; }
    public ContentTypeEnum ContentType { get; }

    private GetResourcesResult(ApiPageResponseBase<T> resources)
    {
        PagedResourceItems = resources;
        CsvResult = Option<CsvExportResponse>.None;
        ContentType = ContentTypeEnum.Json;
    }

    private GetResourcesResult(Option<CsvExportResponse> csvResult)
    {
        CsvResult = csvResult;
        ContentType = ContentTypeEnum.Csv;
    }

    public static GetResourcesResult<T> FromJson(ApiPageResponseBase<T> response)
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));

        return new GetResourcesResult<T>(response);
    }

    public static GetResourcesResult<T> FromCsv(Option<CsvExportResponse> csvResult)
    {
        return new GetResourcesResult<T>(csvResult);
    }
}
