using System.Text.Json;
using System.Text.Json.Serialization;

#nullable disable

namespace NoFrixion.MoneyMoov.Models.Utils
{
    public class PageResponse<T>
    {
        [JsonPropertyName("content")]
        public List<T> Content { get; set; } = new();

        /// <summary>
        /// Current page number. Its 1 based. i.e firstpage is 1, secondpage is 2 
        /// </summary>
        /// <value>Current page number. Its 1 based. i.e first page is 1, second page is 2</value>
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        /// <value>Page size</value>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        /// <value>Total pages</value>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Total count
        /// </summary>
        /// <value>Total count</value>
        [JsonPropertyName("totalSize")]
        public long TotalSize { get; set; }
    }
}
