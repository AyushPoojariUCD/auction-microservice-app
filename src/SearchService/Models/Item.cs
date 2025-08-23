using MongoDB.Entities;

namespace SearchService;

public class Item : Entity
{
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string Color { get; set; } = default!;

}