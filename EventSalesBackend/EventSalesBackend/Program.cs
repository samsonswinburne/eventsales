using Auth0.AspNetCore.Authentication;
using EventSalesBackend.Data;
using EventSalesBackend.Exceptions.Configuration;
using EventSalesBackend.Repositories.Implementation;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Implementation;
using EventSalesBackend.Services.Interfaces;
using FluentValidation;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Map ObjectId to string in Swagger
    c.MapType<ObjectId>(() => new OpenApiSchema { Type = "string" });

    // Map GeoJsonPoint to a simple object
    c.MapType<GeoJsonPoint<GeoJson2DGeographicCoordinates>>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["latitude"] = new() { Type = "number", Format = "double" },
            ["longitude"] = new() { Type = "number", Format = "double" }
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

// data
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IHostRepository, HostRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
// services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IHostService, HostService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();
builder.Services.AddScoped<IGeocodeService, GeocodeService>();
var app = builder.Build();

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