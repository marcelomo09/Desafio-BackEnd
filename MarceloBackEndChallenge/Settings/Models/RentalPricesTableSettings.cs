public class RentalPricesTableSettings
{
    public string Name { get; set; }
    
    public List<RentalPriceTableSettingsValue> Prices { get; set; }

    public RentalPricesTableSettings()
    {
        Name   = string.Empty;
        Prices = new List<RentalPriceTableSettingsValue>();
    }
}

public class RentalPriceTableSettingsValue
{
    public int Days { get; set; }
    public float Price { get; set; }
    public float AssessmentPercent { get; set; }
}