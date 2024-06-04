public class DeliverymanResponse
{
    public string IdDeliveryman { get; set; }

    public string Name { get; set; }

    public string CNPJ { get; set; }

    public string DateOfBirth { get; set; }

    public string CNH { get; set; }

    public string TypeCNH { get; set; }

    public string ImageCNHPath { get; set; }

    public DeliverymanResponse()
    {
        IdDeliveryman = string.Empty;
        Name          = string.Empty;
        CNPJ          = string.Empty;
        CNH           = string.Empty;
        DateOfBirth   = string.Empty;
        TypeCNH       = string.Empty;
        ImageCNHPath  = string.Empty;
    }

    public DeliverymanResponse(Deliveryman deliveryman)
    {
        IdDeliveryman = deliveryman.Id.ToString();
        Name          = deliveryman.Name;
        CNPJ          = deliveryman.CNPJ;
        CNH           = deliveryman.CNH;
        DateOfBirth   = DateTime.Parse(deliveryman.DateOfBirth.ToString()).ToString("dd/MM/yyyy");
        TypeCNH       = deliveryman.TypeCNH;
        ImageCNHPath  = deliveryman.ImageCNHPath;
    }
}