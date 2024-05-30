using MongoDB.Bson;
using MongoDB.Driver;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly MongoDbContext _context;

    public MotorcycleRepository(MongoDbContext context)
    {
        _context = context;

        // Cria o index de informação unica do campo Plate
        var indexKeysDefinition = Builders<Motorcycle>.IndexKeys.Ascending(x => x.Plate);
        var indexOpts           = new CreateIndexOptions { Unique = true };
        var modelIndex          = new CreateIndexModel<Motorcycle>(indexKeysDefinition, indexOpts);

        _context.Motorcycles.Indexes.CreateOne(modelIndex);
    }

    public async Task CreateMotorcycle(Motorcycle motorcycle) => await _context.Motorcycles.InsertOneAsync(motorcycle);

    public async Task DeleteMotorcycle(string id) => await _context.Motorcycles.DeleteOneAsync(x => x.Id == id);

    public async Task<Motorcycle> GetMotorcycle(string id) => await _context.Motorcycles.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Motorcycle> GetMotorcycleForPlate(string plate) => await _context.Motorcycles.Find(x => x.Plate == plate).FirstOrDefaultAsync();

    public async Task<List<Motorcycle>> GetMotorcycles() => await _context.Motorcycles.Find(x => true).ToListAsync();

    public async Task<bool> MotorcyclePlateExists(string id, string plate) => await _context.Motorcycles.Find(x => x.Id != id && x.Plate == plate).AnyAsync();

    public async Task UpdateMotorcycle(Motorcycle motorcycle) => await _context.Motorcycles.ReplaceOneAsync(x => x.Id == motorcycle.Id, motorcycle);
}