namespace SearchService.RequestHelpers
{
    public class SearchParams
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        
        // Specific filters
        public string? Seller { get; set; }
        public string? Winner { get; set; }
        public string? Status { get; set; }
        
        // Sorting & high-level filtering
        public string? OrderBy { get; set; }
        public string? FilterBy { get; set; }

        // Optional numeric filters
        public int? MinReservePrice { get; set; }
        public int? MaxReservePrice { get; set; }
        public int? MinBid { get; set; }
        public int? MaxBid { get; set; }
    }
}
