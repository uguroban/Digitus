using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    
    public bool IsAdmin { get; set; }
    public string? VerificationCode { get; set; }
    public string? PasswordResetCode { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? VerifiedAt { get; set; } 
}