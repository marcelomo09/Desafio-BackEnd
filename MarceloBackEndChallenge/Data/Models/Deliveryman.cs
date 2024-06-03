using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class Deliveryman
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string IdDeliveryman { get => Id.ToString(); }

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

    [BsonElement("DateOfBirth")]
    [Required(ErrorMessage = "Data de nascimento não informada")]
    [BsonRequired]
    [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
    public Date DateOfBirth { get; set; }

    [BsonElement("CNH")]
    [Required(ErrorMessage = "CNH não informada")]
    [BsonRequired]
    [MaxLength(11)]
    [MinLength(11)]
    [RegularExpression(@"\d{11}", ErrorMessage = "O tamanho da CNH são de 9 caracteres, favor digitar apenas os números")]
    public string CNH { get; set; }

    [BsonElement("TypeCNH")]
    [Required(ErrorMessage = "Tipo da CNH não informada")]
    [BsonRequired]
    [MaxLength(3)]
    [StringLength(3, ErrorMessage = "O máximo de caracteres para o Tipo de CNH são 3")]
    [ValidateTypeCNH]
    public string TypeCNH { get; set; }

    [BsonElement("ImageCNHPath")]
    [Required(ErrorMessage = "Foto não enviada")]
    [BsonRequired]
    public string ImageCNHPath { get; set; }

    [BsonElement("PhoneNumber")]
    [Required(ErrorMessage = "Celular não informado")]
    [BsonRequired]
    [MaxLength(11)]
    [MinLength(11)]
    [RegularExpression(@"\d{11}", ErrorMessage = "Favor informar apenas os números do celular com DDD")]
    public string PhoneNumber { get; set; }
    
    public Deliveryman()
    {
        Name         = string.Empty;
        CNPJ         = string.Empty;
        DateOfBirth  = new Date(DateTime.UtcNow);
        CNH          = string.Empty;
        TypeCNH      = string.Empty;
        ImageCNHPath = string.Empty;
        PhoneNumber  = string.Empty;
    }

    public Deliveryman(string imageCNHPath, CreateDeliverymanRequest request)
    {
        Name         = request.Name;
        CNPJ         = request.CNPJ;
        DateOfBirth  = new Date(DateTime.Parse(request.DateOfBirth));
        CNH          = request.CNH;
        TypeCNH      = request.TypeCNH;
        ImageCNHPath = imageCNHPath;
        PhoneNumber  = request.PhoneNumber;
    }
}