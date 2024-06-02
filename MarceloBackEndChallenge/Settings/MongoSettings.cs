public class MongoSettings
{
    public string ConnectionString { get; set; }
    
    public string DatabaseName { get; set; }

    public MongoCollections Collections { get; set; }

    public MongoSettings()
    {
        ConnectionString = string.Empty;
        DatabaseName     = string.Empty;
        Collections      = new MongoCollections();
    }
}