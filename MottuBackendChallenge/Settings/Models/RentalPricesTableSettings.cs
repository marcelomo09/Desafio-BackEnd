public class RentalPricesTableSettings
{
    public string Name { get; set; }
    
    public List<RentalPricesTableRentalPricesTableSettingsValue> Prices { get; set; }

    public RentalPricesTableSettings()
    {
        Name   = string.Empty;
        Prices = new List<RentalPricesTableRentalPricesTableSettingsValue>();
    }
}

public class RentalPricesTableRentalPricesTableSettingsValue
{
    public int Days { get; set; }
    public float Price { get; set; }
}