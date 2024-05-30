using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Motorcycle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    public string? Id { get; set; }

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
    [Required(ErrorMessage = "Placa da moto não informada")]
    [StringLength(7, ErrorMessage = "Tamanho máximo de caracteres é 7")]
    public string Plate { get; set; }

    public Motorcycle()
    {
        Year  = 0;
        Model = string.Empty;
        Plate = string.Empty;
    }

    public Motorcycle(CreateMotorcycleParams param)
    {
        Year  = param.Year;
        Model = param.Model;
        Plate = param.Plate;
    }
}