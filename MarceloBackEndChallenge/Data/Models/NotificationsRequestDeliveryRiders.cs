using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class NotificationsRequestDeliveryRiders
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    [BsonElement("IdRequestRace")]
    public string IdRequestRace { get; set; }

    [BsonElement("IdDeliveryman")]
    public string IdDeliveryman { get; set; }

    [BsonElement("Name")]
    [Required(ErrorMessage = "Nome do entregador não informado")]
    [BsonRequired]
    [MaxLength(50)]
    [StringLength(50, ErrorMessage = "Tamanho máximo de caracteres é 50")]
    public string Name { get; set; }

    [BsonElement("CNPJ")]
    [Required(ErrorMessage = "CNPJ do entregador não informado")]
    [BsonRequired]
    [MaxLength(14)]
    [RegularExpression(@"\d{14}", ErrorMessage = "O CNPJ deve ter exatamente 14 dígitos, favor digitar apenas os números")]
    [MinLength(14)] 
    public string CNPJ { get; set; }

    [BsonElement("CNH")]
    [Required(ErrorMessage = "CNH não informada")]
    [BsonRequired]
    [MaxLength(11)]
    [MinLength(11)]
    [RegularExpression(@"\d{11}", ErrorMessage = "O tamanho da CNH são de 9 caracteres, favor digitar apenas os números")]
    public string CNH { get; set; }

    [BsonElement("RequestRaceValue")]
    [BsonRequired]
    [Required]
    public float RequestRaceValue { get; set; }

    public NotificationsRequestDeliveryRiders()
    {
        IdRequestRace = string.Empty;
        IdDeliveryman = string.Empty;
        Name          = string.Empty;
        CNPJ          = string.Empty;
        CNH           = string.Empty;
    }

    public NotificationsRequestDeliveryRiders(string idRequestRace, float requestRaceValue, Deliveryman deliveryman)
    {
        Id               = ObjectId.GenerateNewId();
        IdRequestRace    = idRequestRace;
        IdDeliveryman    = deliveryman.IdDeliveryman;
        Name             = deliveryman.Name;
        CNPJ             = deliveryman.CNPJ;
        CNH              = deliveryman.CNH;
        RequestRaceValue = requestRaceValue;
    }
}