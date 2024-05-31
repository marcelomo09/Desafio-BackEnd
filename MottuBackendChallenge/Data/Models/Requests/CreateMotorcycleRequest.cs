using System.ComponentModel.DataAnnotations;

public class CreateMotorcycleRequest
{
    [Required(ErrorMessage = "Ano da moto não informada")]
    public int    Year  { get; set; }

    [Required(ErrorMessage = "Modelo da moto não informada")]
    public string Model { get; set;}

    [Required(ErrorMessage = "Placa da moto não informada")]
    [RegularExpression(@"\d{7}", ErrorMessage = "Favor informar as 7 letras e números que compoem a placa")]
    public string Plate { get; set; }

    public int    InUse { get; set; }

    public CreateMotorcycleRequest()
    {
        Model = string.Empty;
        Plate = string.Empty;
    }
}