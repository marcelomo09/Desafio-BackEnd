public interface IDeliverymanRepository
{
    Task<List<Deliveryman>> GetDeliveryDrivers();
    Task<Deliveryman> GetDeliveryman(string id);
    Task CreateDeliveryman(Deliveryman deliveryman);
    Task UpdateDeliveryman(Deliveryman deliveryman);
    Task DeleteDeliveryman(string id);
    Task<bool> CNPJExists(string id, string cnpj);
    Task<bool> CNHExists(string id, string cnh);
}