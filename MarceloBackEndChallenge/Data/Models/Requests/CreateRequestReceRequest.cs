using System.ComponentModel.DataAnnotations;

public class CreateRequestReceRequest
{
    [Required(ErrorMessage = "Favor inserir o número do pedido")]
    public string RequestNum { get; set;}

    [Required(ErrorMessage = "Favor dar detalhes sobre o pedido")]
    public string Description { get; set;}

    [Required(ErrorMessage = "Favor informar o valor da corrida")]
    public float RaceValue { get; set; }

    public CreateRequestReceRequest()
    {
        RequestNum  = string.Empty;
        Description = string.Empty;
    }
}