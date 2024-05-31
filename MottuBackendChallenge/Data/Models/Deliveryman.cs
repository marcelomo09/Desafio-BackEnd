using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class Deliveryman
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    public string? Id { get; set; }

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

    public Deliveryman()
    {
        Name         = string.Empty;
        CNPJ         = string.Empty;
        DateOfBirth  = new Date();
        CNH          = string.Empty;
        TypeCNH      = string.Empty;
        ImageCNHPath = string.Empty;
    }

    public Deliveryman(CreateDeliverymanRequest deliveryman, string imagepath)
    {
        Name         = deliveryman.Name;
        CNPJ         = deliveryman.CNPJ;
        DateOfBirth  = new Date(DateTime.Parse(deliveryman.DateOfBirth));
        CNH          = deliveryman.CNH;
        TypeCNH      = deliveryman.TypeCNH;
        ImageCNHPath = imagepath;
    }

    public Deliveryman(UpdateDeliverymanRequest deliveryman, string imagepath)
    {
        Id           = deliveryman.Id;
        Name         = deliveryman.Name;
        CNPJ         = deliveryman.CNPJ;
        DateOfBirth  = new Date(DateTime.Parse(deliveryman.DateOfBirth));
        CNH          = deliveryman.CNH;
        TypeCNH      = deliveryman.TypeCNH;
        ImageCNHPath = imagepath;
    }
}