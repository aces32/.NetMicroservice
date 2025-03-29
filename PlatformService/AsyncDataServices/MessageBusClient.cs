using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IModel? _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"]
                                ?? throw new ArgumentNullException("RabbitMQHost"),
                Port = int.Parse(_configuration["RabbitMQPort"]
                                 ?? throw new ArgumentNullException("RabbitMQPort"))
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                
                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not create a connection: {ex.Message}");
            }
        }


        public void PublishNewPlatform(PlatformPublishedDto? platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection?.IsOpen == true)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
            }   
        }

        private void SendMessage(string message)
        {
            if (_channel?.IsOpen == true)
            {
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = false;
                _channel.BasicPublish(exchange: "trigger",
                                      routingKey: "",
                                      basicProperties: properties,
                                      body: body);
                Console.WriteLine($"--> We have sent {message}");
            }
            else
            {
                Console.WriteLine("--> RabbitMQ channel is closed");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");

            if (_channel?.IsOpen == true)
                _channel.Close();

            if (_connection?.IsOpen == true)
                _connection.Close();
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
