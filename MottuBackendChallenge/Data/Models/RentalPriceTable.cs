using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class RentalPriceTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    public string? Id { get; set; }

    [BsonElement("Days")]
    [BsonRequired]
    [Required]
    public int Days { get; set; }

    [BsonElement("Price")]
    [BsonRequired]
    [Required]
    public float Price { get; set; }

    [BsonElement("AssessmentPercent")]
    [BsonRequired]
    [Required]
    public float AssessmentPercent { get; set; }
}