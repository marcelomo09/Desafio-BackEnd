public interface IMotorcycleRentalRepository
{
    Task<List<MotorcycleRental>> GetMotorcycleRentals();
    Task<MotorcycleRental> GetMotorcycleRental(string id);
    Task CreateMotorcycleRental(MotorcycleRental entity);
    Task UpdateMotorcycleRental(MotorcycleRental entity);
    Task DeleteMotorcycleRental(string id);
}