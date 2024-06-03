using System.ComponentModel.DataAnnotations;

public class CreateRequestReceRequest
{
    [Required(ErrorMessage = "Favor informar o valor da corrida para o pedido")]
    public float RequestRaceValue { get; set; }

    public CreateRequestReceRequest()
    {
        
    }
}