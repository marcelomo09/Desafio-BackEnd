using System.Reflection.Emit;
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
    }

    /// <summary>
    /// Método utilizado quando projeto e levantado, para garantir que as collections existam, também há tratamentos de inclusão inicial de valores
    /// </summary>
    /// <returns></returns>
    public async Task Initialized()
    {
        await VerifyIfNotExistsCollectionAndCreatedAsync(_settings.Value.Collections.Users);
        await VerifyIfNotExistsCollectionAndCreatedAsync(_settings.Value.Collections.Motorcycles);
        await VerifyIfNotExistsCollectionAndCreatedAsync(_settings.Value.Collections.DeliveryDrivers);
        await VerifyIfNotExistsCollectionAndCreatedAsync(_settings.Value.Collections.MotorcycleRentals);

        await InitializeRentalPricesTable();

        await CreateIndexes();
    }

    #region Private Methods

    private async Task<bool> VerifyIfNotExistsCollectionAndCreatedAsync(string name)
    {
        var collecttionNames = await _database.ListCollectionNamesAsync();

        bool collectionExists = collecttionNames.ToList().  Contains(name);

        if (!collectionExists)
        {
            _database.CreateCollection(name);
        }

        return collectionExists;
    }

    /// <summary>
    /// Inicializa a tabela RentalPricesTable com valores já pré-definidos no AppSettings
    /// </summary>
    private async Task InitializeRentalPricesTable()
    {
        await VerifyIfNotExistsCollectionAndCreatedAsync(_settings.Value.Collections.RentalPricesTable.Name);

        await RentalPricesTable.DeleteManyAsync(x => true);

        foreach (var item in _settings.Value.Collections.RentalPricesTable.Prices)
        {
            await RentalPricesTable.InsertOneAsync(new RentalPriceTable() { Days = item.Days, Price = item.Price, AssessmentPercent = item.AssessmentPercent });
        }
    }

    private CreateIndexModel<T> CreateModelIndexUnique<T>(IndexKeysDefinition<T> indexkey)
    {
        var indexOpts  = new CreateIndexOptions { Unique = true };
        var modelIndex = new CreateIndexModel<T>(indexkey, indexOpts);

        return modelIndex;
    }


    private async Task CreateIndexes()
    {
        await Motorcycles.Indexes.CreateOneAsync(CreateModelIndexUnique<Motorcycle>(Builders<Motorcycle>.IndexKeys.Ascending(x => x.Plate)));

        await DeliveryDrivers.Indexes.CreateOneAsync(CreateModelIndexUnique<Deliveryman>(Builders<Deliveryman>.IndexKeys.Ascending(x => x.CNH)));
        await DeliveryDrivers.Indexes.CreateOneAsync(CreateModelIndexUnique<Deliveryman>(Builders<Deliveryman>.IndexKeys.Ascending(x => x.CNPJ)));
    }

    #endregion Private Methods

    #region  Declare Colletions

    public IMongoCollection<User> Users => _database.GetCollection<User>(_settings.Value.Collections.Users);
    public IMongoCollection<Motorcycle> Motorcycles => _database.GetCollection<Motorcycle>(_settings.Value.Collections.Motorcycles);
    public IMongoCollection<Deliveryman> DeliveryDrivers => _database.GetCollection<Deliveryman>(_settings.Value.Collections.DeliveryDrivers);
    public IMongoCollection<RentalPriceTable> RentalPricesTable => _database.GetCollection<RentalPriceTable>(_settings.Value.Collections.RentalPricesTable.Name);
    public IMongoCollection<MotorcycleRental> MotorcycleRentals => _database.GetCollection<MotorcycleRental>(_settings.Value.Collections.MotorcycleRentals);

    #endregion Declare Colletions
}