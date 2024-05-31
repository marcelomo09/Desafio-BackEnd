using ZstdSharp.Unsafe;

public class MotorcycleService
{
    private readonly MotorcycleRepository _motorcycleRepository;

    public MotorcycleService(MotorcycleRepository repos)
    {
        _motorcycleRepository = repos;
    }

    public async Task<Response> CreateMotorcycle(Motorcycle motorcycle)
    {
        bool plateexists = await PlateExists(string.Empty, motorcycle.Plate);

        if (plateexists) return new Response(true, "Placa já encontra-se cadastrada!", ResponseTypeResults.BadRequest);

        await _motorcycleRepository.CreateMotorcycle(motorcycle);

        return new Response(false, "Moto cadastrada com sucesso!");
    }

    public async Task<Response> DeleteMotorcycle(string id)
    {
        var moto = await _motorcycleRepository.GetMotorcycle(id);

        if (moto == null) return new Response(true, "Moto não encontrada!", ResponseTypeResults.NotFound);

        if (moto.InUse != 0) return new Response(true, "Moto não pode ser excluída, pois a mesma está alugada.", ResponseTypeResults.BadRequest);

        await _motorcycleRepository.DeleteMotorcycle(id);

        return new Response(false, "Moto excluída com sucesso!");
    }

    public async Task<Motorcycle> GetMotorcycle(string id)
    {
        return await _motorcycleRepository.GetMotorcycle(id);
    }

    public async Task<List<Motorcycle>> GetMotorcycles()
    {
        return await _motorcycleRepository.GetMotorcycles();
    }

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

    public async Task<Motorcycle> GetMotorcycleForPlate(string plate)
    {
        return await _motorcycleRepository.GetMotorcycleByPlate(plate);
    }

    private async Task<bool> PlateExists(string id, string plate)
    {
        return await _motorcycleRepository.MotorcyclePlateExists(id, plate);
    }
}