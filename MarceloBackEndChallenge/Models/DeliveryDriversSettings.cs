public class DeliveryDriversSettings
{
    public string Name { get; set; }

    public List<string> TypesCNH { get; set; }

    public DeliveryDriversSettings()
    {
        Name     = string.Empty;
        TypesCNH = new List<string>();
    }
}