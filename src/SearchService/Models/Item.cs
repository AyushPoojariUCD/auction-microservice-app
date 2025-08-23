using MongoDB.Entities;

namespace SearchService;

public class Item : Entity
{
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int ReservePrice { get; set; }
    public string Seller { get; set; } = default!;
    public string Winner { get; set; } = default!;
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; }  = DateTime.UtcNow; 
    public DateTime UpdatedAt { get; set; }  = DateTime.UtcNow; 
    public DateTime AuctionEnd { get; set; }
    public string Status { get; set; } = default!;

}