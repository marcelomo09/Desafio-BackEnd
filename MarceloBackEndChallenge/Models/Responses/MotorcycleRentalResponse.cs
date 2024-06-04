public class MotorcycleRentalResponse
{
    public string IdMotorcycleRental { get; set; }

    public string IdDeliveryman { get; set; }

    public string IdMotocycle { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    public string ExpectedEndDate { get; set; }

    public int PlanOfLocation { get; set; }

    public float TotalValueExpected { get; set; }

    public float TotalValue { get; set; }

    public int Active { get; set; }

    public MotorcycleRentalResponse()
    {
        IdMotorcycleRental = string.Empty;
        IdDeliveryman      = string.Empty;
        IdMotocycle        = string.Empty;
        StartDate          = string.Empty;
        EndDate            = string.Empty;
        ExpectedEndDate    = string.Empty;
    }

    public MotorcycleRentalResponse(MotorcycleRental motorcycleRental)
    {
        IdMotorcycleRental = motorcycleRental.IdMotorcycleRental;
        IdDeliveryman      = motorcycleRental.IdDeliveryman;
        IdMotocycle        = motorcycleRental.IdMotocycle;
        StartDate          = DateTime.Parse(motorcycleRental.StartDate.ToString()).ToString("dd/MM/yyyy HH:mm");
        EndDate            = DateTime.Parse(motorcycleRental.EndDate.ToString()).ToString("dd/MM/yyyy HH:mm");
        ExpectedEndDate    = DateTime.Parse(motorcycleRental.ExpectedEndDate.ToString()).ToString("dd/MM/yyyy HH:mm");
        PlanOfLocation     = motorcycleRental.PlanOfLocation;
        TotalValueExpected = motorcycleRental.TotalValueExpected;
        TotalValue         = motorcycleRental.TotalValue;
        Active             = motorcycleRental.Active;
    }
}