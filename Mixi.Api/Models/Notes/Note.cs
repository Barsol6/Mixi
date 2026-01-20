using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mixi.Api.Modules.Notes;

public class Note
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required] public string Name { get; set; }

    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserName { get; set; }
    public DateTime UpdatedAt { get; set; }
}