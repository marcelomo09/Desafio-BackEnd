using MongoDB.Driver;

public class DeliverymanRepository : IDeliverymanRepository
{
    private readonly MongoDbContext _context;

    public DeliverymanRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CNHExists(string? id, string cnh) => await _context.DeliveryDrivers.Find(x => x.Id != id && x.CNH == cnh).AnyAsync();

    public async Task<bool> CNPJExists(string? id, string cnpj) => await _context.DeliveryDrivers.Find(x => x.Id != id && x.CNPJ == cnpj).AnyAsync();

    public async Task CreateDeliveryman(Deliveryman deliveryman) => await _context.DeliveryDrivers.InsertOneAsync(deliveryman);

    public async Task DeleteDeliveryman(string id) => await _context.DeliveryDrivers.DeleteOneAsync(x => x.Id == id);

    public Task<List<Deliveryman>> GetDeliveryDrivers() => _context.DeliveryDrivers.Find(x => true).ToListAsync();

    public Task<Deliveryman> GetDeliveryman(string id) => _context.DeliveryDrivers.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task UpdateDeliveryman(Deliveryman deliveryman)
    {
        var deliverymanExisting = await GetDeliveryman(deliveryman.Id ?? string.Empty);

        if (deliverymanExisting == null) return;

        if (string.IsNullOrEmpty(deliveryman.ImageCNHPath))
        {
            deliveryman.ImageCNHPath = deliverymanExisting.ImageCNHPath;
        }

        await _context.DeliveryDrivers.ReplaceOneAsync(x => x.Id == deliveryman.Id, deliveryman);
    }
}