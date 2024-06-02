using System.ComponentModel.DataAnnotations;

public class SimulationOfMotorcycleRentalValuesRequest
{
    [Required(ErrorMessage = "Data inicial da alocação no foi informada")]
    [ValidDate("dd/MM/yyyy HH:mm", true, ErrorMessage = "A Data de inicio deve estar no formato dd/MM/yyyy HH:mm.")]
    public string StartDate { get; set; }

    [Required(ErrorMessage = "Data de entrega da locação não foi informada")]
    [ValidDate("dd/MM/yyyy HH:mm", true, ErrorMessage = "A Data de entrega deve estar no formato dd/MM/yyyy HH:mm.")]
    [CompareDatesMoreOrLessThen("StartDate", ErrorMessage = "Data de entrega não pode ser menor que a data inicial.")]
    public string EndDate { get; set; }

    [Required(ErrorMessage = "Favor informar o plano.")]
    public int PlanOfLocation { get; set; }

    public SimulationOfMotorcycleRentalValuesRequest()
    {
        StartDate = string.Empty;
        EndDate   = string.Empty;
    }
}