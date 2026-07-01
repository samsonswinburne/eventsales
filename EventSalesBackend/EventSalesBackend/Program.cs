using Auth0.AspNetCore.Authentication;
using EventSalesBackend.Data;
using EventSalesBackend.Exceptions.Configuration;
using EventSalesBackend.Options;
using EventSalesBackend.Pipelines.Implementation;
using EventSalesBackend.Pipelines.Interfaces;
using EventSalesBackend.Repositories.Implementation;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Implementation;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Map ObjectId to string in Swagger
    c.MapType<ObjectId>(() => new OpenApiSchema { Type = Microsoft.OpenApi.JsonSchemaType.String });

    // Map GeoJsonPoint to a simple object
    c.MapType<GeoJsonPoint<GeoJson2DGeographicCoordinates>>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.Object,

        Properties = new Dictionary<string, IOpenApiSchema>
        {
            ["latitude"] = new OpenApiSchema { Type = JsonSchemaType.Number, Format = "double" },
            ["longitude"] = new OpenApiSchema { Type = JsonSchemaType.Number, Format = "double" }
        }
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(options => { options.MinimumSameSitePolicy = SameSiteMode.None; });

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    var requiredOptions = builder.Configuration.GetRequiredSection("Auth0");
    // needs a dto lmaoooo
    options.Domain = requiredOptions["Domain"] ?? throw new EventSalesMongoConfigurationException("Domain");
    options.ClientId = requiredOptions["ClientId"] ?? throw new EventSalesMongoConfigurationException("ClientId");
});

builder.Services.Configure<RedisOptions>(
    builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;

    return ConnectionMultiplexer.Connect(options.ConnectionString);
});
builder.Services.AddSingleton<IDistributedLockProvider>(sp =>
{
    var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return new RedisDistributedSynchronizationProvider(multiplexer.GetDatabase());
});
// data
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IMongoResiliencePipelineProvider, MongoResiliencePipelineProvider>();
builder.Services.AddTransient<ICryptoService, CryptoService>();
// repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IHostRepository, HostRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ISeatHoldRepository, SeatHoldRepository>();
// services
builder.Services.AddHttpClient();
builder.Services.AddTransient<ISessionProvider, SessionProvider>();
builder.Services.AddScoped<IRequestCompanyAdminRepository, RequestCompanyAdminRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IHostService, HostService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();
builder.Services.AddScoped<IGeocodeService, GeocodeService>();
builder.Services.AddScoped<ISeatHoldService, SeatHoldService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<ISeatLockService, SeatLockService>();

builder.Services.AddSingleton<IPayPalClientService, PayPalClientService>();
builder.Services.AddScoped<IPayPalService, PayPalService>();
var app = builder.Build();



// Force MongoDbContext to initialize
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
    Console.WriteLine("MongoDbContext initialized");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();