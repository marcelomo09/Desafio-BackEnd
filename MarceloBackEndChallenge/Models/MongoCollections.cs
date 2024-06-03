public class MongoCollections
{
    public string Motorcycles { get; set; }

    public DeliveryDriversSettings DeliveryDrivers { get; set; }

    public string MotorcycleRentals { get; set; }

    public UserSettings Users { get; set; }

    public RentalPricesTableSettings RentalsPricesTable { get; set; }

    public RequestRidesSettings RequestRides { get; set; }

    public MongoCollections()
    {
        Motorcycles        = string.Empty;
        DeliveryDrivers    = new DeliveryDriversSettings();
        MotorcycleRentals  = string.Empty;
        Users              = new UserSettings();
        RentalsPricesTable = new RentalPricesTableSettings();
        RequestRides       = new RequestRidesSettings();
    }
}