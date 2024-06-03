using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class MotorcycleRental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string IdMotorcycleRental { get => Id.ToString(); }

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

    [BsonElement("TotalValueExpected")]
    [BsonRequired]
    [Required]
    public float TotalValueExpected { get; set; }

    [BsonElement("TotalValue")]
    [BsonRequired]
    [Required]
    public float TotalValue { get; set; }

    [BsonElement("Active")]
    [BsonRequired]
    [Required]
    public int Active { get; set; }

    public MotorcycleRental()
    {
        IdDeliveryman   = string.Empty;
        IdMotocycle     = string.Empty;
        StartDate       = new Date(DateTime.UtcNow);
        EndDate         = new Date(DateTime.UtcNow);
        ExpectedEndDate = new Date(DateTime.UtcNow);
    }

    public MotorcycleRental(string idDeliveryman, string idMotorcycle, Date startDate, Date expectedDate, float totalValue, CreateMotorcycleRentalRequest request)
    {
        IdDeliveryman      = idDeliveryman;
        IdMotocycle        = idMotorcycle;
        StartDate          = startDate;
        EndDate            = new Date(DateTime.Parse(request.EndDate));
        ExpectedEndDate    = expectedDate;
        PlanOfLocation     = request.PlanOfLocation;
        TotalValueExpected = totalValue;
        Active             = 1;
    }
}