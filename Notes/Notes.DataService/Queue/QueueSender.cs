using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Notes.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notes.DataService
{
    internal class QueueSender : IQueueSender
    {
        private readonly IConnection _connection;
        private readonly string QueueName;
        public QueueSender(string queueName, string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
            _connection = factory.CreateConnection();
            QueueName = queueName;
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
