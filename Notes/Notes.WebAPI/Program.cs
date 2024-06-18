using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Bson;
using Notes.DI;
using Notes.Model;
using Notes.WebAPI;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddSingleton<IQueue>(z => new Queue(QueueSettings.QueueName, QueueSettings.HostName, QueueSettings.UserName, QueueSettings.Password));
            //builder.Services.AddHostedService<RabbitMQBackgroundService>();
            //builder.Services.AddSingleton(RabbitHutch.CreateBus($"host={QueueSettings.HostName};username={QueueSettings.UserName};password={QueueSettings.Password}"));

            //builder.Services.AddSingleton<IMessageHandler, OrderMessageHandler>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();            
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }        
    }
}
