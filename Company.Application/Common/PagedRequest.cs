using System.ComponentModel.DataAnnotations;

namespace Company.Application.Common
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        [RegularExpression("^(asc|desc)$", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
        public string SortOrder { get; set; } = "asc";

        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
    }
}