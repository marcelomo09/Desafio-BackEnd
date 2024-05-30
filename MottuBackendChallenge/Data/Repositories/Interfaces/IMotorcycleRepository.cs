public interface IMotorcycleRepository
{
    Task<List<Motorcycle>> GetMotorcycles();
    Task<Motorcycle> GetMotorcycle(string id);
    Task CreateMotorcycle(Motorcycle motorcycle);
    Task UpdateMotorcycle(Motorcycle motorcycle);
    Task DeleteMotorcycle(string id);
    Task<bool> MotorcyclePlateExists(string id, string plate);
    Task<Motorcycle> GetMotorcycleForPlate(string plate);
}