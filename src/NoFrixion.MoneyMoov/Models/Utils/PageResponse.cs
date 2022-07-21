using Newtonsoft.Json;

#nullable disable

namespace NoFrixion.MoneyMoov.Models.Utils
{
    public class PageResponse<T>
    {
        [JsonProperty("content")]
        public T Content { get; set; }

        /// <summary>
        /// Current page number. Its 0 based. i.e firstpage &#x3D;0, secondpage&#x3D;1 
        /// </summary>
        /// <value>Current page number. Its 0 based. i.e firstpage &#x3D;0, secondpage&#x3D;1 </value>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        /// <value>Page size</value>
        [JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        /// <value>Total pages</value>
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Total count
        /// </summary>
        /// <value>Total count</value>
        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        public override string ToString()
        {
            return $"{nameof(Content)}: {Content}, {nameof(Page)}: {Page}, {nameof(Size)}: {Size}, {nameof(TotalPages)}: {TotalPages}, {nameof(TotalSize)}: {TotalSize}";
        }
    }
}
