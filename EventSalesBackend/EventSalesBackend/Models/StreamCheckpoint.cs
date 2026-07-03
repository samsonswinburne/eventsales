using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventSalesBackend.Models;

public class StreamCheckpoint
{
    [BsonId] public string WatcherName { get; set; } = default!;
    public string InstanceId { get; set; }
    public BsonDocument? ResumeToken  { get; set; }
}