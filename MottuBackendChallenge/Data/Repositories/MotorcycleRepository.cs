using MongoDB.Bson;
using MongoDB.Driver;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly MongoDbContext _context;

    public MotorcycleRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateMotorcycle(Motorcycle motorcycle) => await _context.Motorcycles.InsertOneAsync(motorcycle);

    public async Task DeleteMotorcycle(string id) => await _context.Motorcycles.DeleteOneAsync(x => x.Id == id);

    public async Task<Motorcycle> GetMotorcycle(string id) => await _context.Motorcycles.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Motorcycle> GetMotorcycleByPlate(string plate) => await _context.Motorcycles.Find(x => x.Plate == plate).FirstOrDefaultAsync();

    public async Task<List<Motorcycle>> GetMotorcycles() => await _context.Motorcycles.Find(x => true).ToListAsync();

    public async Task<bool> MotorcyclePlateExists(string id, string plate) => await _context.Motorcycles.Find(x => ( string.IsNullOrEmpty(id) || x.Id != id ) && x.Plate == plate).AnyAsync();

    public async Task UpdateMotorcycle(Motorcycle motorcycle) => await _context.Motorcycles.ReplaceOneAsync(x => x.Id == motorcycle.Id, motorcycle);

    public async Task<List<Motorcycle>> GetMotorcylclesDinponibles() => await _context.Motorcycles.Find(x => x.InUse == 0).ToListAsync();

    public async Task<bool> MotorcycleInUse(string? id) => await _context.Motorcycles.Find(x => x.Id == id && x.InUse == 1).AnyAsync();
}