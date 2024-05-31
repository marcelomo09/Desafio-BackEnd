public class MongoCollections
{
    public string Users { get; set; }

    public string Motorcycles { get; set; }

    public string DeliveryDrivers { get; set; }

    public string MotorcycleRentals { get; set; }

    public RentalPricesTableSettings RentalPricesTable { get; set; }

    public MongoCollections()
    {
        Users             = string.Empty;
        Motorcycles       = string.Empty;
        DeliveryDrivers   = string.Empty;
        MotorcycleRentals = string.Empty;
        RentalPricesTable = new RentalPricesTableSettings();
    }
}