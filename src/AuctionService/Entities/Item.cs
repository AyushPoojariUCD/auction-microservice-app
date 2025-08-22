using System;
using System.ComponentModel.DataAnnotations.Schema;
using AuctionService.Entities;

namespace AuctionService.Entities
{
    [Table("Items")]  // Maps this entity to the "Items" table
    public class Item
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }

        // Relationship
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }
    }
}
