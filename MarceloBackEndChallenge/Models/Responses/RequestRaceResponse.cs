public class RequestRaceResponse
{
    public string IdRequestRace { get; set; }

    public string IdMotorcycleRental { get; set; }

    public float RequestRaceValue { get; set; }

    public string Situation { get; set; }

    public RequestRaceResponse()
    {
        IdRequestRace      = string.Empty;
        IdMotorcycleRental = string.Empty;
        Situation          = string.Empty;
    }

    public RequestRaceResponse(RequestRace requestRace)
    {
        IdRequestRace      = requestRace.IdRequestRace;
        IdMotorcycleRental = requestRace.IdMotorcycleRental ?? string.Empty;
        RequestRaceValue   = requestRace.RequestRaceValue;
        Situation          = requestRace.Situation;
    }
}