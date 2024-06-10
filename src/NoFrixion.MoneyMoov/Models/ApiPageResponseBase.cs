// -----------------------------------------------------------------------------
//  Filename: ApiPageResponseBase.cs
// 
//  Description: Contains details of a PageResponse model:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  10 Feb 2022  Arif Matin   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models.Utils;
using System.Text;

namespace NoFrixion.MoneyMoov.Models;

public class BeneficiaryPageResponse : ApiPageResponseBase<Beneficiary>
{
    public BeneficiaryPageResponse(List<Beneficiary> content,
        int pageNumber = 1,
        int pageSize = 10,
        int totalPages = default,
        long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public class BeneficiaryGroupPageResponse : ApiPageResponseBase<BeneficiaryGroup>
{
    public BeneficiaryGroupPageResponse(List<BeneficiaryGroup> content,
        int pageNumber = 1,
        int pageSize = 10,
        int totalPages = default,
        long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public class MerchantTokenPageResponse : ApiPageResponseBase<MerchantToken>
{
    public MerchantTokenPageResponse(List<MerchantToken> content,
                int pageNumber = 1,
                int pageSize = 10,
                int totalPages = default,
                long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public class TransactionPageResponse : ApiPageResponseBase<Transaction>
{
    public TransactionPageResponse(List<Transaction> content,
        int pageNumber = 1,
        int pageSize = 10,
        int totalPages = default,
        long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public class PaymentAccountPageResponse : ApiPageResponseBase<PaymentAccount>
{
    public PaymentAccountPageResponse(List<PaymentAccount> content,
        int pageNumber = 1,
        int pageSize = 10,
        int totalPages = default,
        long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public class MerchantPageResponse : ApiPageResponseBase<Merchant>
{
    public MerchantPageResponse(List<Merchant> content,
        int pageNumber = 1,
        int pageSize = 10,
        int totalPages = default,
        long totalSize = default)
        : base(content, pageNumber, pageSize, totalPages, totalSize)
    { }
}

public abstract class ApiPageResponseBase<T> : PageResponse<T>
{
    /// <summary>
    /// Initializes a new instance PayoutsPageResponse of the  class.
    /// </summary>
    /// <param name="content">List of payouts on the current page (required).</param>
    /// <param name="pageNumber">Current page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="totalPages">Total pages.</param>
    /// <param name="totalSize">Total count.</param>
    protected ApiPageResponseBase(List<T> content,
            int pageNumber = 1,
            int pageSize = 10,
            int totalPages = default,
            long totalSize = default)
    {
        // to ensure "content" is required (not null)
        Content = content ?? throw new ArgumentNullException("content is a required property for PageResponse and cannot be null");
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalSize = totalSize;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class PageResponse {\n");
        sb.Append("  Content: ").Append(Content).Append("\n");
        sb.Append("  PageNumber: ").Append(PageNumber).Append("\n");
        sb.Append("  PageSize: ").Append(PageSize).Append("\n");
        sb.Append("  TotalPages: ").Append(TotalPages).Append("\n");
        sb.Append("  TotalSize: ").Append(TotalSize).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
        => this.ToJsonFormatted();
}