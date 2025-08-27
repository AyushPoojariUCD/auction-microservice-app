using Polly;
using System.Net;
using MassTransit;
using SearchService;

using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

builder.Services.AddMassTransit(
    x =>
    {
        // RabbitMQ adding consumer:: AuctionCreatedConsumer
        x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

        x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

        
        
        x.UsingRabbitMq((context, cfg) =>
        {
             cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
            {
                h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
            });

            // Handling Fault :: MongoDB Down
            cfg.ReceiveEndpoint("search-auction-created", e =>
            {
                e.UseMessageRetry(r => r.Interval(5, 5));

                e.ConfigureConsumer<AuctionCreatedConsumer>(context);
            });

            cfg.ConfigureEndpoints(context);
        });
    }
);

var app = builder.Build();

// Middleware
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.initDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
