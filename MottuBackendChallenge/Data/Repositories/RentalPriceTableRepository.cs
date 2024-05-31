using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class RentalPriceTableRepository : IRentalPriceTableRepository
{
    private readonly MongoDbContext _context;

    public RentalPriceTableRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateRentalPriceTable(RentalPriceTable entity) => await _context.RentalPricesTable.InsertOneAsync(entity);

    public async Task DeleteRentalPriceTable(string id) => await _context.RentalPricesTable.DeleteOneAsync(x => x.Id == id);

    public async Task<List<RentalPriceTable>> GetRentalPricesTable() => await _context.RentalPricesTable.Find(x => true).ToListAsync();

    public async Task<RentalPriceTable> GetRentalPriceTable(string id) => await _context.RentalPricesTable.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<RentalPriceTable> GetRentalPriceTableForDay(int day) => await _context.RentalPricesTable.Find(x => x.Days == day).FirstOrDefaultAsync();

    public async Task<RentalPriceTable> GetRentalPriceTableForPrice(float price) => await _context.RentalPricesTable.Find(x => x.Price == price).FirstOrDefaultAsync();

    public Task UpdateRentalPriceTable(RentalPriceTable entity) => _context.RentalPricesTable.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    public async Task<RentalPriceTable> GetPlanForDaySelected(int days) => await _context.RentalPricesTable.Find(x => x.Days == days).FirstOrDefaultAsync();
}