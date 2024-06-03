using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    [BsonRequired]
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string IdUser { get => Id.ToString(); }

    [Required]
    [BsonRequired]
    [BsonElement("UserName")]
    [MaxLength(20)]
    [RegularExpression(@"^\S*$", ErrorMessage = "O nome de usuário não pode conter espaços.")]
    public string UserName { get; set; }

    [Required]
    [BsonRequired]
    [BsonElement("Password")]
    [JsonIgnore]
    public string Password { get; set; }

    [Required]
    [BsonRequired]
    [BsonElement("CreateDate")]
    public Date CreateDate { get; set; }

    [Required]
    [BsonRequired]
    [BsonElement("UserGroup")]
    public string UserGroup { get; set; }

    public User()
    {
        UserName   = string.Empty;
        Password   = string.Empty;
        CreateDate = new Date(DateTime.UtcNow);
        UserGroup  = string.Empty;
    }

    public User(CreateUserRequest request)
    {
        UserName   = request.UserName;
        Password   = CryptoSHA256.ConvertStringToSHA256(request.Password);
        CreateDate = new Date(DateTime.UtcNow);
        UserGroup  = request.UserGroup;
    }
}