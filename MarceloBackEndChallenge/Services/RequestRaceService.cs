using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using RabbitMQ.Client;

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

    #region Private Methods

    private Response PublishMessagesForNotifications(string idRequestRace, List<Deliveryman> deliveryDrivers)
    {
        try
        {
            if (deliveryDrivers == null || deliveryDrivers.Count == 0) return new Response();

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var notification = new NotificationsDeliveryRiders(idRequestRace, deliveryDrivers);

                    channel.QueueDeclare(queue: notification.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var message = Newtonsoft.Json.JsonConvert.SerializeObject(notification);
                    var body    = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: notification.Queue,
                                         basicProperties: null,
                                         body: body);
                }
            }

            return new Response();
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção na montagem da fila de notificações: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    private async Task<Response> PublishDeliveryDriversForNotification(string idRequestRace)
    {
        try
        {
            // Busca todos os pedidos de locação ativa e verifica se as mesma não estão em algum pedido já aceito
            var motorcycleRentals  = await _dbContext.MotorcycleRentals.Where(x => x.Active == 1).ToListAsync();
            var requestRidesAccept = await _dbContext.RequestRides.Where(x => x.Situation == _settings.Value.Collections.RequestRides.Situations.Accept).ToListAsync();

            var rentalsNotInRequests = motorcycleRentals.Where(x => requestRidesAccept == null || requestRidesAccept.Count == 0 || !requestRidesAccept.Any(a => a.IdMotorcycleRental == x.Id.ToString())).ToList();

            // Encerra o processamento de notificações por não ter entregadores e motos alugadas disponiveis para o pedido
            if (rentalsNotInRequests == null || rentalsNotInRequests.Count == 0) return new Response();

            // Busca apenas os entregadores com motos alugadas e que ainda não pegaram um pedido
            var deliveryDrivers = await _dbContext.DeliveryDrivers.Where(d => rentalsNotInRequests.Any(r => r.IdDeliveryman == d.Id.ToString())).ToListAsync();

            // Publica no RabbitMQ as mensagens a serem enviadas para os entregadores
            var response = PublishMessagesForNotifications(idRequestRace, deliveryDrivers);

            return response;
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção durante a busca dos entregadores para envio de notificação do pedido de corrida criado: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    #endregion Private Methods

    #region  Public Methods

    public async Task<Response> Create(CreateRequestReceRequest request)
    {        
        try
        {
            // Inclusão do pedido
            var requestRace = new RequestRace(_settings.Value.Collections.RequestRides.Situations.Disponible, request);
            
            await _dbContext.RequestRides.AddAsync(requestRace);

            await _dbContext.SaveChangesAsync();

            // TODO: Realizar a correção e ajustar a rotina de notificações
            // var response = await PublishDeliveryDriversForNotification(requestRace.IdRequestRace);

            // if (response.Error) response.Message = $"O pedido foi registrado com sucesso! Porem ocorreram problemas na montagem de notificações: {response.Message}";

            // return response.Error ? response : new Response(false, "Pedido registrado com sucesso!");

            return new Response(false, "Pedido registrado com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção na criação do pedido: {ex.Message}", ResponseTypeResults.BadRequest);
        }   
    }
    
    public async Task<Response> Accept(string idRequestRace, string cnh)
    {
        try
        {
            // Busca o entregador pela CNH
            var deliveryman = await _dbContext.DeliveryDrivers.Where(x => x.CNH == cnh).FirstOrDefaultAsync();

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.NotFound);

            // Busca a moto alugada pelo entregador
            var motorcycleRental = await _dbContext.MotorcycleRentals.Where(x => x.IdDeliveryman == deliveryman.IdDeliveryman).FirstOrDefaultAsync();

            if (motorcycleRental == null) return new Response(true, "Entregador não possuí nenhuma moto alugada.", ResponseTypeResults.BadRequest);

            // Busca a corrida informada pela identificação
            var race = await _dbContext.RequestRides.Where(x => x.Id.ToString() == idRequestRace).FirstOrDefaultAsync();

            if (race == null) return new Response(true, "Pedido de corrida não encontrado.", ResponseTypeResults.NotFound);

            if (race.Situation != _settings.Value.Collections.RequestRides.Situations.Disponible) return new Response(true, "Pedido não encontra-se disponivel.", ResponseTypeResults.BadRequest);

            race.IdMotorcycleRental = motorcycleRental.IdMotorcycleRental;
            race.Situation          = _settings.Value.Collections.RequestRides.Situations.Accept;

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Pedido de corrida aceito!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção durante a aceitação do pedido: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    public async Task<Response> Deliver(string idRequestRace, string cnh)
    {
        try
        {
            // Busca o entregador pela CNH
            var deliveryman = await _dbContext.DeliveryDrivers.Where(x => x.CNH == cnh).FirstOrDefaultAsync();

            if (deliveryman == null) return new Response(true, "Entregador não encontrado.", ResponseTypeResults.NotFound);

            // Busca a moto alugada pelo entregador
            var motorcycleRental = await _dbContext.MotorcycleRentals.Where(x => x.IdDeliveryman == deliveryman.IdDeliveryman).FirstOrDefaultAsync();

            if (motorcycleRental == null) return new Response(true, "Entregador não possuí nenhuma moto alugada.", ResponseTypeResults.BadRequest);

            // Busca a corrida informada pela identificação
            var race = await _dbContext.RequestRides.Where(x => x.Id.ToString() == idRequestRace).FirstOrDefaultAsync();

            if (race == null) return new Response(true, "Pedido de corrida não encontrado.", ResponseTypeResults.NotFound);

            if (race.Situation != _settings.Value.Collections.RequestRides.Situations.Accept) return new Response(true, "Pedido não pode ser entregue, pois o mesmo não encontra-se com status de aceito.", ResponseTypeResults.BadRequest);

            if (race.IdMotorcycleRental != motorcycleRental.IdMotorcycleRental) return new Response(true, "Pedido não pode ser entregue por outro entregador", ResponseTypeResults.BadRequest);

            race.Situation = _settings.Value.Collections.RequestRides.Situations.Deliver;

            await _dbContext.SaveChangesAsync();

            return new Response(false, "Pedido entregue com sucesso!");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exceção durante a entrega do pedido: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }

    #endregion Public Methods
}