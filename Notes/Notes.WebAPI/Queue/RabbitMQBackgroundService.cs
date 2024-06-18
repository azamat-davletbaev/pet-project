using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Notes.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notes.WebAPI
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IMessageHandler _messageHandler;
        public RabbitMQBackgroundService(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = QueueSettings.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QueueSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var s = Encoding.UTF8.GetString(body);
                
                _messageHandler.HandleMessageAsync(s).Wait();
            };

            _channel.BasicConsume(queue: QueueSettings.QueueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            await base.StopAsync(cancellationToken);
        }
    }
}
