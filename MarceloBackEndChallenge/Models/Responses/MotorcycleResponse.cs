public class MotorcycleResponse
{
    public string IdMotorcycle { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }

    public MotorcycleResponse()
    {
        IdMotorcycle = string.Empty;
        Model        = string.Empty;
        Plate        = string.Empty;
    }

    public MotorcycleResponse(Motorcycle motorcycle)
    {
        IdMotorcycle = motorcycle.Id.ToString();
        Year         = motorcycle.Year;
        Model        = motorcycle.Model;
        Plate        = motorcycle.Plate;
    }
}