using Contracts;
using AutoMapper;
using MassTransit;
using AuctionService;
using AuctionService.DTOs;
using AuctionService.Data;
using AuctionService.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    // GET: api/auctions
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt > DateTime.Parse(date).ToUniversalTime());
        }

        // var auctions = await _context.Auctions
        //         .Include(x => x.Item)
        //         .OrderBy(x => x.Item.Make)
        //         .ToListAsync();

        // return Ok(_mapper.Map<List<AuctionDto>>(auctions));

        // return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();

        var auctions = await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();

        return Ok(auctions);
    }

    // GET: api/auctions/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        return Ok(_mapper.Map<AuctionDto>(auction));
    }

    [Authorize]
    [HttpPost]
    // POST: api/auctions
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync() > 0;

        var newAuction = _mapper.Map<AuctionDto>(auction);

        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById),
            new { auction.Id }, newAuction);
    }

    // PUT: api/auctions/{id}
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctiondto)
    {

        var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        auction.Item.Make = updateAuctiondto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctiondto.Model ?? auction.Item.Model;
        auction.Item.Year = updateAuctiondto.Year = auction.Item.Year;
        auction.Item.Color = updateAuctiondto.Color = auction.Item.Color;
        auction.Item.Mileage = updateAuctiondto.Mileage = auction.Item.Mileage;

        var result = await _context.SaveChangesAsync() > 0;

        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

        if (result) return Ok();

        return BadRequest("Bad Request");
    }

    // DELETE: api/auctions/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        _context.Auctions.Remove(auction);

        await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}

