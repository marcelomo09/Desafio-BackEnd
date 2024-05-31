using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Entities;

public class MotorcycleRental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    public string? Id { get; set; }

    [BsonElement("IdDeliveryman")]
    [BsonRequired]
    [Required]
    public string IdDeliveryman { get; set; }

    [BsonElement("IdMotocycle")]
    [BsonRequired]
    [Required]
    public string IdMotocycle { get; set; }

    [BsonElement("StartDate")]
    [BsonRequired]
    [Required]
    public Date StartDate { get; set; }

    [BsonElement("EndDate")]
    [BsonRequired]
    [Required]
    public Date EndDate { get; set; }

    [BsonElement("ExpectedEndDate")]
    [BsonRequired]
    [Required]
    public Date ExpectedEndDate { get; set; }

    [BsonElement("PlanOfLocation")]
    [BsonRequired]
    [Required]
    public int PlanOfLocation { get; set; }

    [BsonElement("TotalValue")]
    [BsonRequired]
    [Required]
    public float TotalValue { get; set; }

    public MotorcycleRental()
    {
        IdDeliveryman   = string.Empty;
        IdMotocycle     = string.Empty;
        StartDate       = new Date(DateTime.UtcNow);
        EndDate         = new Date(DateTime.UtcNow);
        ExpectedEndDate = new Date(DateTime.UtcNow);
    }

    public MotorcycleRental(string iddeliveryman, string idmotorcycle, Date expecteddate, float totalvalue, CreateMotorcycleRentalRequest request)
    {
        IdDeliveryman   = iddeliveryman;
        IdMotocycle     = idmotorcycle;
        StartDate       = new Date(DateTime.Parse(request.StartDate));
        EndDate         = new Date(DateTime.Parse(request.EndDate));
        ExpectedEndDate = expecteddate;
        PlanOfLocation  = request.PlanOfLocation;
        TotalValue      = totalvalue;
    }
}