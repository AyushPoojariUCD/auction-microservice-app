using AutoMapper;
using Contracts;
using MassTransit;
using AuctionService.Entities;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    private readonly IMapper _mapper;

    public AuctionCreatedFaultConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Consuming faulty creation");

        var exception = context.Message.Exceptions.First();

        // Map the actual message, not the fault wrapper
        if (exception.ExceptionType == "System.ArgumentException")
        {
            // Modify and republish safely
            var newMessage = context.Message.Message;
            newMessage.Model = "FooBar";
            await context.Publish(newMessage);
        }
        else
        {
            Console.WriteLine("Not an argument exception - update error dashboard somewhere");
        }
    }
}
