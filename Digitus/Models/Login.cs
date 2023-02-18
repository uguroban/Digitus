using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Models;

public class Login
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Email { get; set; }
    
    public bool IsAdmin { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime LoginStartTime { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? LoginEndTime { get; set; }
}