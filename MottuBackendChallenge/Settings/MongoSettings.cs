public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }

    public MongoCollections Collections { get; set; }

    public MongoSettings()
    {
        ConnectionString = string.Empty;
        Database         = string.Empty;
        Collections      = new MongoCollections();
    }
}

public class MongoCollections
{
    public string Users { get; set; }

    public string Motorcycles { get; set; }

    public string DeliveryDrivers { get; set; }

    public MongoCollections()
    {
        Users           = string.Empty;
        Motorcycles     = string.Empty;
        DeliveryDrivers = string.Empty;
    }
}