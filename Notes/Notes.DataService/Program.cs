using DataService.Tables;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.DI;
using DataService.Data;

namespace DataService
{
    internal class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        static void Main(string[] args)
        {
            using (var ctx = new PostgresContext())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IDataProvider>(sp=> new DataProvider(ctx));

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var db = ServiceProvider.GetService<IDataProvider>();
                var users = db.GetAllUsers();
            }
        }        
    }
}
