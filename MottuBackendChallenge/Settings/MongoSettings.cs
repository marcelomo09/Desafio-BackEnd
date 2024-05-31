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

