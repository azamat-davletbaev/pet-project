using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notes.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notes.DataService
{
    public class Queue : IQueue
    {
        private readonly IConnection _connection;
        private readonly IModel channel;
        private readonly string QueueName;
        public event Action<string> OnReceivered;
        
        public Queue(string queueName, string hostName, string userName, string password)
        {
            QueueName = queueName;

            var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
            _connection = factory.CreateConnection();
            channel = _connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void SendMessage(string message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                byte[] body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
            }
        }
    }
}
