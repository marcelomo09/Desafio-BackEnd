public class CreateMotorcycleParams
{
    public int    Year  { get; set; }

    public string Model { get; set;}

    public string Plate { get; set; }

    public CreateMotorcycleParams()
    {
        Year  = 0;
        Model = string.Empty;
        Plate = string.Empty;
    }
}