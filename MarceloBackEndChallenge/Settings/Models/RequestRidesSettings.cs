public class RequestRidesSettings
{
    public string Name { get; set; }

    public SituationRequestRidesSettings Situations { get; set; }

    public RequestRidesSettings()
    {
        Name       = string.Empty;
        Situations = new SituationRequestRidesSettings();
    }
}

public class SituationRequestRidesSettings
{
    public string Disponible { get; set; }

    public string Accept { get; set; }

    public string Deliivered { get; set; }

    public SituationRequestRidesSettings()
    {
        Disponible = string.Empty;
        Accept     = string.Empty;
        Deliivered = string.Empty;
    }
}