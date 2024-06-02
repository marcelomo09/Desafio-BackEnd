using System.ComponentModel.DataAnnotations;

public class CreateMotorcycleRentalRequest
{
    [Required(ErrorMessage = "CNH não informada")]
    [RegularExpression(@"\d{11}", ErrorMessage = "O tamanho da CNH são de 11 caracteres, favor digitar apenas os números")]
    public string CNH { get; set; }

    [Required(ErrorMessage = "Placa da moto não informada")]
    [RegularExpression(@"^[A-Za-z0-9]{7}$", ErrorMessage = "Favor informar as 7 letras e números que compoem a placa")]
    public string Plate { get; set; }

    [Required(ErrorMessage = "Data final da locação não informada")]
    [ValidDate("dd/MM/yyyy HH:mm", true, ErrorMessage = "A Data final deve estar no formato dd/MM/yyyy HH:mm.")]
    public string EndDate { get; set; }

    [Required(ErrorMessage = "Favor informar o plano.")]
    public int PlanOfLocation { get; set; }

    public CreateMotorcycleRentalRequest()
    {
        CNH       = string.Empty;
        Plate     = string.Empty;
        EndDate   = string.Empty;
    }
}