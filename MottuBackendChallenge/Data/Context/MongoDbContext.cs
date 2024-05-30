using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;

public class MongoDbContext: DBContext
{
    private readonly IMongoDatabase _database;

    private readonly IOptions<MongoSettings> _settings;

    public MongoDbContext(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);

        _database = client.GetDatabase(settings.Value.Database);

        _settings = settings;

        VerifyIfNotExistsCollectionAndCreated(_settings.Value.Collections.Users);
        VerifyIfNotExistsCollectionAndCreated(_settings.Value.Collections.Motorcycles);
        VerifyIfNotExistsCollectionAndCreated(_settings.Value.Collections.DeliveryDrivers);
    }

    public void VerifyIfNotExistsCollectionAndCreated(string name)
    {
        var collecttionExists = _database.ListCollectionNames().ToList().Contains(name);

        if (!collecttionExists)
        {
            _database.CreateCollection(name);
        }
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>(_settings.Value.Collections.Users);
    public IMongoCollection<Motorcycle> Motorcycles => _database.GetCollection<Motorcycle>(_settings.Value.Collections.Motorcycles);
    public IMongoCollection<Deliveryman> DeliveryDrivers => _database.GetCollection<Deliveryman>(_settings.Value.Collections.DeliveryDrivers);
}