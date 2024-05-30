using System.ComponentModel.DataAnnotations;

public class UpdateDeliveryman
{
    [Required(ErrorMessage = "Número identificador do entregador e necessário")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Nome do entregador não informado")]
    [StringLength(50, ErrorMessage = "Tamanho máximo de caracteres é 50")]
    public string Name { get; set; }

    [Required(ErrorMessage = "CNPJ do entregador não informado")]
    [RegularExpression(@"\d{14}", ErrorMessage = "O CNPJ deve ter exatamente 14 dígitos, favor digitar apenas os números")]
    public string CNPJ { get; set; }

    [Required(ErrorMessage = "Data de nascimento não informada")]
    [ValidDate("dd/MM/yyyy", ErrorMessage = "A data deve estar no formato dd/MM/yyyy.")]
    public string DateOfBirth { get; set; }

    [Required(ErrorMessage = "CNH não informada")]
    [RegularExpression(@"\d{11}", ErrorMessage = "O tamanho da CNH são de 11 caracteres, favor digitar apenas os números")]
    public string CNH { get; set; }

    [Required(ErrorMessage = "Tipo da CNH não informada")]
    [StringLength(3, ErrorMessage = "O máximo de caracteres para o Tipo de CNH são 3")]
    [ValidateTypeCNH]
    public string TypeCNH { get; set; }

    public IFormFile? ImageCNH { get; set; }

    public UpdateDeliveryman()
    {
        Id          = string.Empty;
        Name        = string.Empty;
        CNPJ        = string.Empty;
        DateOfBirth = string.Empty;
        CNH         = string.Empty;
        TypeCNH     = string.Empty;
    }
}