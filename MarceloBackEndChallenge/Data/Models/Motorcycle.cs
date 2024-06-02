using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Motorcycle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string IdMotorcycle { get { return Id.ToString(); } }

    [BsonElement("Year")]
    [Required(ErrorMessage = "Ano da moto não informada")]
    [BsonRequired]
    public int Year { get; set; }

    [BsonElement("Model")]
    [BsonRequired]
    [MaxLength(25)]
    [Required(ErrorMessage = "Modelo da moto não informada")]
    [StringLength(25, ErrorMessage = "Tamanho máximo de caracteres é 25")]
    public string Model { get; set; }

    [BsonElement("Plate")]
    [BsonRequired]
    [MaxLength(7)]
    [MinLength(7)]
    [Required(ErrorMessage = "Placa da moto não informada")]
    public string Plate { get; set; }

    public Motorcycle()
    {
        Model = string.Empty;
        Plate = string.Empty;
    }

    public Motorcycle(CreateMotorcycleRequest request)
    {
        Year  = request.Year;
        Model = request.Model;
        Plate = request.Plate;
    }
}