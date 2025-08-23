using AutoMapper;
using MongoDB.Driver;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;
using SearchService.RequestHelpers;
namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item,Item>();

        // query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        // Apply ordering
        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new"  => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _      => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        // Apply filtering
        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),

            "endingsoon" => query.Match(x =>
                x.AuctionEnd > DateTime.UtcNow &&
                x.AuctionEnd <= DateTime.UtcNow.AddHours(6)
            ),

            _ => query
        };

        // Apply filters
        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }
        
        
        // Pagination
        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
