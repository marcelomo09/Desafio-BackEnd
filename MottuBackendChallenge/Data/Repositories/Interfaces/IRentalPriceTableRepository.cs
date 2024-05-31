public interface IRentalPriceTableRepository
{
    Task<List<RentalPriceTable>> GetRentalPricesTable();
    Task<RentalPriceTable> GetRentalPriceTable(string id);
    Task<RentalPriceTable> GetRentalPriceTableForDay(int day);
    Task<RentalPriceTable> GetRentalPriceTableForPrice(float price);
    Task CreateRentalPriceTable(RentalPriceTable value);
    Task UpdateRentalPriceTable(RentalPriceTable value);
    Task DeleteRentalPriceTable(string id);
}