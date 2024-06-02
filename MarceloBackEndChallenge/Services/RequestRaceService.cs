using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class RequestRaceService : ServiceBase
{
    private readonly IOptions<MongoSettings> _settings;

    public RequestRaceService(MongoDBContext mongoDBContext, IOptions<MongoSettings> settings) : base(mongoDBContext)
    {
        _settings = settings;
    }

    /// <summary>
    /// Busca todos os pedidos já cadastrados
    /// </summary>
    /// <returns></returns>
    public async Task<List<RequestRace>> GetAll()
    {
        return await _dbContext.RequestRides.ToListAsync();
    }

    public async Task<Response> Create(CreateRequestReceRequest request)
    {        
        try
        {
            // TODO: Rotina do envio de sms

            await _dbContext.RequestRides.AddAsync(new RequestRace(_settings.Value.Collections.RequestRides.Situations.Disponible, request));

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Pedido registrado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção na criação do pedido: {ex.Message}", ResponseTypeResults.BadRequest);
        }   
    }
}