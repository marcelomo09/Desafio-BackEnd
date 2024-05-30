using System.ComponentModel.DataAnnotations;

public class CreateUserParams
{
    [Required(ErrorMessage = "Nome não informado")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Senha não informada")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Level não informado")]
    public string Level { get; set; }

    public CreateUserParams()
    {
        Name     = string.Empty;
        Password = string.Empty;
        Level    = string.Empty;
    }
}