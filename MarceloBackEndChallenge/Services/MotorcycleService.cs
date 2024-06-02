using Microsoft.EntityFrameworkCore;

public class MotorcycleService: ServiceBase
{
    public MotorcycleService(MongoDBContext mongoDBContext) : base(mongoDBContext)
    {

    }

    /// <summary>
    /// Busca uma lista de todas as motos cadastradas
    /// </summary>
    /// <returns>Retorno da lista de todas motos caddastradas</returns>
    public async Task<List<Motorcycle>> GetAll()
    {
        return await _dbContext.Motorcycles.ToListAsync();
    }

    /// <summary>
    /// Cadastra uma moto no mongodb
    /// </summary>
    /// <param name="request">Dados de cadastro da moto</param>
    /// <returns>Retorna uma resposta do processo de cadastro da moto</returns>
    public async Task<Response> Create(CreateMotorcycleRequest request)
    {
        bool plateExists = await _dbContext.Motorcycles.Where(x => x.Plate == request.Plate).AnyAsync();

        if (plateExists) return new Response(true, "Placa já encontra-se cadastrada em outra moto.", ResponseTypeResults.BadRequest);

        await _dbContext.Motorcycles.AddAsync(new Motorcycle(request));

        await _dbContext.SaveChangesAsync();

        return new Response(false, "Moto cadastrada com sucesso!");
    }

    /// <summary>
    /// Excluí uma moto cadastrada
    /// </summary>
    /// <param name="id">Identificação da moto</param>
    /// <returns>Retorna uma resposta do processo de exclusão da moto</returns>
    public async Task<Response> Delete(string plate)
    {
        var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == plate);

        if (motorcycle == null) return new Response(true, "Moto não encontrada.", ResponseTypeResults.NotFound);

        // Verifica se a moto encontra-se em uso
        var rentals = await _dbContext.MotorcycleRentals.FirstOrDefaultAsync(x => x.IdMotocycle == motorcycle.IdMotorcycle && x.Active == 1);

        if (rentals != null) return new Response(true, "Moto não pode ser atualizada, pois está alocada.", ResponseTypeResults.BadRequest);

        _dbContext.Motorcycles.Remove(motorcycle);

        await _dbContext.SaveChangesAsync();

        return new Response(false, "Moto excluída com sucesso!");
    }

    /// <summary>
    /// Alteração da placa da moto
    /// </summary>
    /// <param name="oldPlate">Placa antiga</param>
    /// <param name="newPlate">Placa nova</param>
    /// <returns>Retorna uma resposta do processo de alteração da placa da moto</returns>
    public async Task<Response> UpdatePlate(string oldPlate, string newPlate)
    {
        var motorcycle = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == oldPlate) ?? new Motorcycle();

        if (string.IsNullOrEmpty(motorcycle.Plate)) return new Response(true, "Moto não encontrada pela placa antiga.", ResponseTypeResults.NotFound);

        var plateExists = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == newPlate);

        if (plateExists != null) return new Response(true, $"Placa já cadastrada na moto {plateExists.Model}", ResponseTypeResults.BadRequest);

        // Verifica se a moto encontra-se em uso
        var rentals = await _dbContext.MotorcycleRentals.FirstOrDefaultAsync(x => x.IdMotocycle == motorcycle.IdMotorcycle && x.Active == 1);

        if (rentals != null) return new Response(true, "Moto não pode ser atualizada, pois está alocada.", ResponseTypeResults.BadRequest);

        motorcycle.Plate = newPlate;

        await _dbContext.SaveChangesAsync();

        return new Response(false, "Placa da moto alterada com sucesso!");
    }

    /// <summary>
    /// Busca uma moto com a placa informada
    /// </summary>
    /// <param name="plate">Placa da moto</param>
    /// <returns>Retorna os dados da moto</returns>
    public async Task<Motorcycle> GetMotorcycleByPlate(string plate)
    {
        var response = await _dbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == plate);

        return response ?? new Motorcycle();
    }

    
}