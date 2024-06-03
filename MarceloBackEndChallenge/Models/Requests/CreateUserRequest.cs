using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Favor informar o nome do usuário")]
    [StringLength(20, ErrorMessage = "Tamanho máximo de caracteres é 20")]
    [RegularExpression(@"^\S*$", ErrorMessage = "O nome de usuário não pode conter espaços.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Favor informar a senha")]
    [RegularExpression(@"^[A-Za-z0-9]{8}$", ErrorMessage = "Senha deve ter 8 caracteres compostos por números e/ou letras")]
    public string Password { get; set; }

    public CreateUserRequest()
    {
        UserName = string.Empty;
        Password = string.Empty;
    }
}