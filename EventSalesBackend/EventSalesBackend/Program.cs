using Auth0.AspNetCore.Authentication;
using EventSalesBackend.Data;
using EventSalesBackend.Repositories.Implementation;
using EventSalesBackend.Repositories.Interfaces;
using EventSalesBackend.Services.Implementation;
using EventSalesBackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CookiePolicyOptions>(options =>
  {
      options.MinimumSameSitePolicy = SameSiteMode.None;
  });

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    var requiredOptions = builder.Configuration.GetRequiredSection("Auth0");
    options.Domain = requiredOptions["Domain"];
    options.ClientId = requiredOptions["ClientId"];
});

// data
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

// repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();

// services
builder.Services.AddSingleton<IEventService, EventService>();

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
