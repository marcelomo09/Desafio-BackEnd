using MongoDB.Driver;

public class MotorcycleRentalRepository : IMotorcycleRentalRepository
{
    private readonly MongoDbContext _context;

    public MotorcycleRentalRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateMotorcycleRental(MotorcycleRental entity) => await _context.MotorcycleRentals.InsertOneAsync(entity);

    public async Task DeleteMotorcycleRental(string id) => await _context.MotorcycleRentals.DeleteOneAsync(x => x.Id == id);

    public async Task<MotorcycleRental> GetMotorcycleRental(string id) => await _context.MotorcycleRentals.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<MotorcycleRental>> GetMotorcycleRentals() => await _context.MotorcycleRentals.Find(x => true).ToListAsync();

    public async Task UpdateMotorcycleRental(MotorcycleRental entity) => await _context.MotorcycleRentals.ReplaceOneAsync(x => x.Id == entity.Id, entity);
}