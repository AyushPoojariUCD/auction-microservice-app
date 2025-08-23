using AutoMapper;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
namespace SearchService;

public class AuctionSvcHttpClient
{
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient; // assign parameter to field
            _config = config;         // assign parameter to field
        }

        public async Task<List<Item>> GetItemsForSearchDb()
        {
        var lastUpdated = await DB.Find<Item, String>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
            
            return await _httpClient.GetFromJsonAsync<List<Item>>( _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
        }
}

