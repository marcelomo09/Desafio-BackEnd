using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class RequestRace
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    public ObjectId Id { get; set; }

    public string IdRequestRace { get { return Id.ToString(); } }

    [BsonElement("IdDeliveryman")]
    public string? IdDeliveryman { get; set;}

    [BsonElement("IdMotorcycleRental")]
    public string? IdMotorcycleRental { get; set; }

    [BsonElement("RequestNum")]
    [Required(ErrorMessage = "Favor inserir o número do pedido")]
    [BsonRequired]
    public string RequestNum { get; set;}

    [BsonElement("Description")]
    [Required(ErrorMessage = "Favor dar detalhes sobre o pedido")]
    [BsonRequired]
    public string Description { get; set;}

    [BsonElement("CreateDate")]
    [Required(ErrorMessage = "Data de criação não informada")]
    [BsonRequired]
    [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
    public Date CreateDate { get; set; }

    [BsonElement("RaceValue")]
    [BsonRequired]
    [Required]
    public float RaceValue { get; set; }

    [BsonElement("Situation")]
    [Required(ErrorMessage = "Situation não informada")]
    [BsonRequired]
    [ValidateSituationsRequestRace]
    public string Situation { get; set; }

    public RequestRace()
    {
        RequestNum  = string.Empty;
        Description = string.Empty;
        CreateDate  = new Date();
        Situation   = string.Empty;
    }

    /// <summary>
    /// Constructor utilizado para preencher os campo para criar o pedido de corrida
    /// </summary>
    /// <param name="idDeliveryman">Identificação do entregador</param>
    /// <param name="idMotoRental">Identificação da locação da moto pelo entregador</param>
    /// <param name="request">Dados de requisição para criação do pedido de corrida</param>
    public RequestRace(string situation, CreateRequestReceRequest request)
    {
        RequestNum  = request.RequestNum;
        Description = request.Description;
        CreateDate  = new Date();
        RaceValue   = request.RaceValue;
        Situation   = situation;
    }
}