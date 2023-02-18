using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Digitus.Dtos;

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    
}