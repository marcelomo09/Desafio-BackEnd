public class MongoCollections
{
    public string Motorcycles { get; set; }

    public string DeliveryDrivers { get; set; }

    public string MotorcycleRentals { get; set; }

    public string Users { get; set; }

    public RentalPricesTableSettings RentalsPricesTable { get; set; }

    public RequestRidesSettings RequestRides { get; set; }

    public MongoCollections()
    {
        Motorcycles        = string.Empty;
        DeliveryDrivers    = string.Empty;
        MotorcycleRentals  = string.Empty;
        Users              = string.Empty;
        RentalsPricesTable = new RentalPricesTableSettings();
        RequestRides       = new RequestRidesSettings();
    }
}