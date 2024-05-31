using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string? Id { get; set; }
    
    [BsonElement("Name")]
    [BsonRequired]
    [Required(ErrorMessage = "Nome não informado")]
    public string Name { get; set; }

    [BsonElement("Password")]
    [BsonRequired]
    [Required(ErrorMessage = "Senha não informada")]
    [JsonIgnore]
    public string Password { get; set; }

    [BsonElement("Level")]
    [BsonRequired]
    [Required(ErrorMessage = "Level não informado")]
    public string Level { get; set; }

    public User()
    {
        Name     = string.Empty;
        Password = string.Empty;
        Level    = string.Empty;
    }

    public User(CreateUserRequest param) 
    {
        Name     = param.Name;
        Password = param.Password;
        Level    = param.Level;
    }
}