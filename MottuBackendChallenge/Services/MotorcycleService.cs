using ZstdSharp.Unsafe;

public class MotorcycleService
{
    private readonly MotorcycleRepository _motorcycleRepository;

    public MotorcycleService(MotorcycleRepository repos)
    {
        _motorcycleRepository = repos;
    }

    /// <summary>
    /// Cria o registro da moto
    /// </summary>
    /// <param name="motorcycle">Dados da moto para cadastro</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> CreateMotorcycle(Motorcycle motorcycle)
    {
        bool plateexists = await PlateExists(string.Empty, motorcycle.Plate);

        if (plateexists) return new Response(true, "Placa já encontra-se cadastrada!", ResponseTypeResults.BadRequest);

        await _motorcycleRepository.CreateMotorcycle(motorcycle);

        return new Response(false, "Moto cadastrada com sucesso!");
    }

    /// <summary>
    /// Excluí a moto de identificação informada
    /// </summary>
    /// <param name="id">Número de identificação da moto</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> DeleteMotorcycle(string id)
    {
        var moto = await _motorcycleRepository.GetMotorcycle(id);

        if (moto == null) return new Response(true, "Moto não encontrada!", ResponseTypeResults.NotFound);

        if (moto.InUse != 0) return new Response(true, "Moto não pode ser excluída, pois a mesma está alugada.", ResponseTypeResults.BadRequest);

        await _motorcycleRepository.DeleteMotorcycle(id);

        return new Response(false, "Moto excluída com sucesso!");
    }

    /// <summary>
    /// Busca uma moto cadastrada pelo seu número de identificação
    /// </summary>
    /// <param name="id">Número de identificação da moto</param>
    /// <returns>Retorna os dados da moto</returns>
    public async Task<Motorcycle> GetMotorcycle(string id)
    {
        return await _motorcycleRepository.GetMotorcycle(id);
    }

    /// <summary>
    /// Busca todas as motos cadastradas
    /// </summary>
    /// <returns>Retorno da lista das motos cadastradas</returns>
    public async Task<List<Motorcycle>> GetMotorcycles()
    {
        return await _motorcycleRepository.GetMotorcycles();
    }

    /// <summary>
    /// Realiza a atualização de dados da moto
    /// </summary>
    /// <param name="motorcycle">Dados de atualização da moto</param>
    /// <returns>Retorna um objeto com propriedades que identificam erros ou não</returns>
    public async Task<Response> UpdateMotorcycle(Motorcycle motorcycle)
    {
        var motoExists = await _motorcycleRepository.GetMotorcycle(motorcycle.Id ?? "");

        if (motoExists == null) return new Response(true, "Moto não encontrada!", ResponseTypeResults.NotFound);

        bool plateExists = await PlateExists(motorcycle.Id ?? string.Empty, motorcycle.Plate);

        if (plateExists) return new Response(true, "Placa já encontra-se cadastrada!", ResponseTypeResults.BadRequest);

        if (await _motorcycleRepository.MotorcycleInUse(motorcycle.Id)) return new Response(true, "Dados da moto não podem ser alterados, pois a mesma está alugada.");

        await _motorcycleRepository.UpdateMotorcycle(motorcycle);

        return new Response(false, "Informações da moto atualizada com sucesso!");
    }

    /// <summary>
    /// Busca uma moto pela sua placa
    /// </summary>
    /// <param name="plate">Placa da moto</param>
    /// <returns>Retorna os dados da moto</returns>
    public async Task<Motorcycle> GetMotorcycleForPlate(string plate)
    {
        return await _motorcycleRepository.GetMotorcycleByPlate(plate);
    }

    /// <summary>
    /// Verifica se existe alguma moto cadastrada com a placa informada porém com uma identificação diferente
    /// </summary>
    /// <param name="id">Número de identificação da moto</param>
    /// <param name="plate">Placa da moto</param>
    /// <returns>Retorna verdadeiro caso outra moto já esteja cadastrada com essa placa</returns>
    private async Task<bool> PlateExists(string id, string plate)
    {
        return await _motorcycleRepository.MotorcyclePlateExists(id, plate);
    }
}