using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    
    private IModel _channel;
    private IConnection _connection;

    public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, IServiceProvider serviceProvider)
    {
        _logger          = logger;
        _serviceProvider = serviceProvider;
        var factory      = new ConnectionFactory() { HostName = "localhost" };
        _connection      = factory.CreateConnection();
        _channel         = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var notifications = new NotificationsDeliveryRiders();
        var notificationQueueName = notifications.Queue;

        _channel.QueueDeclare(queue     : notificationQueueName,
                              durable   : false,
                              exclusive : false,
                              autoDelete: false,
                              arguments : null);

        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (model, ea) =>
        {
            var body    = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MongoDBContext>();

                try
                {
                    var notification = JsonConvert.DeserializeObject<NotificationsDeliveryRiders>(message, settings);

                    if (notification != null)
                    {
                        string idRequestRace   = notification.IdRequestRace;
                        float requestRaceValue = notification.RequestRaceValue;
                        
                        List<Deliveryman> deliveryDrivers = notification.DeliveryDrivers;

                        _logger.LogInformation($"Novo pedido registrado");

                        foreach (var driver in deliveryDrivers)
                        {
                            var newNotification = new NotificationsRequestDeliveryRiders(idRequestRace, requestRaceValue, driver);

                            await dbContext.NotificationsRequestDeliveryRiders.AddAsync(newNotification);

                            await dbContext.SaveChangesAsync();

                            _logger.LogInformation($"Notificação de novo pedido no valor de {requestRaceValue.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))} enviada para o entregador {driver.Name}.");
                        }
                    }
                }
                catch (JsonSerializationException ex)
                {
                    _logger.LogError("Erro de desserialização: " + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Erro ao processar a mensagem: " + ex.Message);
                }
            }
        };

        _channel.BasicConsume(queue: notificationQueueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}