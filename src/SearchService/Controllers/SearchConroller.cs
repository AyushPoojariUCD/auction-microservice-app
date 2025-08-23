using AutoMapper;
using MongoDB.Driver;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm)
    {
        var query = DB.Find<Item>();

        // Default sort by Make
        query.Sort(x => x.Ascending(a => a.Make));

        // If search term exists, do full-text search and sort by text score
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(Search.Full, searchTerm)
                 .SortByTextScore();
        }

        var result = await query.ExecuteAsync();

        return Ok(result); 
    }
}
