using EventSalesBackend.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSalesBackend.Data;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetRequiredSection("MongoDb");
        var connectionString = mongoDbSettings["ConnectionString"];
        var databaseName = mongoDbSettings["DatabaseName"];

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);

        CreateIndexes();
    }

    public IMongoCollection<Event> Events => _database.GetCollection<Event>("events");

    public IMongoCollection<EventHost> Hosts => _database.GetCollection<EventHost>("hosts");

    public IMongoCollection<Company> Companies => _database.GetCollection<Company>("companies");

    public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("tickets");

    private void CreateIndexes()
    {
        // need more indexes for different queries that need more
        var eventIndexes = new[]
        {
            new CreateIndexModel<Event>(
            Builders<Event>.IndexKeys
                .Geo2DSphere(e => e.VenueLocation)
                .Ascending(e => e.Status)
                .Ascending(e => e.InPersonEvent)),
            new CreateIndexModel<Event>(
                Builders<Event>.IndexKeys.Ascending(e => e.StartDate)),
            new CreateIndexModel<Event>(
                Builders<Event>.IndexKeys.Ascending(e => e.Status)),
            new CreateIndexModel<Event>(
                Builders<Event>.IndexKeys.Descending(e => e.Summary.TotalSold))
        };
        Events.Indexes.CreateMany(eventIndexes);

        var indexes = Events.Indexes.List().ToListAsync();
        foreach (var index in indexes.Result)
        {
            Console.WriteLine(index.ToJson());
        }
        //var hostIndexes = new[]
        //{

        //};
        var companyIndexes = new[]
        {
            new CreateIndexModel<Company>(
                Builders<Company>.IndexKeys.Ascending(c => c.PostCode))
        };
        Companies.Indexes.CreateMany(companyIndexes);

        var ticketIndexes = new[]
        {
            new CreateIndexModel<Ticket>(
                Builders<Ticket>.IndexKeys.Ascending(t => t.CustomerEmail)),
            new CreateIndexModel<Ticket>(
                Builders<Ticket>.IndexKeys.Ascending(t => t.EventId)),
            new CreateIndexModel<Ticket>(
                Builders<Ticket>.IndexKeys.Ascending(t => t.CustomerId))
        };
        Tickets.Indexes.CreateMany(ticketIndexes);
    }
}