using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class RequestRace
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string IdRequestRace { get => Id.ToString();  }

    [BsonElement("IdMotorcycleRental")]
    public string? IdMotorcycleRental { get; set; }

    [BsonElement("CreateDate")]
    [Required(ErrorMessage = "Data de criação não informada")]
    [BsonRequired]
    [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
    public Date CreateDate { get; set; }

    [BsonElement("RequestRaceValue")]
    [BsonRequired]
    [Required]
    public float RequestRaceValue { get; set; }

    [BsonElement("Situation")]
    [Required(ErrorMessage = "Situation não informada")]
    [BsonRequired]
    [ValidateSituationsRequestRace]
    public string Situation { get; set; }

    public RequestRace()
    {
        CreateDate = new Date();
        Situation  = string.Empty;
    }

    /// <summary>
    /// Constructor utilizado para preencher os campo para criar o pedido de corrida
    /// </summary>
    /// <param name="idDeliveryman">Identificação do entregador</param>
    /// <param name="idMotoRental">Identificação da locação da moto pelo entregador</param>
    /// <param name="request">Dados de requisição para criação do pedido de corrida</param>
    public RequestRace(string situation, CreateRequestReceRequest request)
    {
        Id               = ObjectId.GenerateNewId();
        CreateDate       = new Date(DateTime.UtcNow);
        RequestRaceValue = request.RequestRaceValue;
        Situation        = situation;
    }
}