using EasyNetQ.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Notes.Model;
using System;

namespace Notes.DataService
{
    internal class Program
    {
        public static IServiceProvider ServiceProvider = DI.Services.ServiceProvider;
        static async Task Main(string[] args)
        {
            using (var ctx = new PostgresContext())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IDataProvider>(z => new DataProvider(ctx));
                serviceCollection.AddSingleton<IQueue>(z=> new Queue(QueueSettings.QueueName, QueueSettings.HostName, QueueSettings.UserName, QueueSettings.Password));
                
                ServiceProvider = serviceCollection.BuildServiceProvider();
                var db = ServiceProvider.GetService<IDataProvider>();                                                                
                var queue = ServiceProvider.GetRequiredService<IQueue>();
                
                queue.OnReceivered += async (message) =>
                {                    
                    Console.WriteLine($"01: {message}" );

                    await Task.Run(()=> 
                    {
                        var request = JsonConvert.DeserializeObject<RequestResponse>(message);

                        if (request.Head == nameof(IDataProvider.GetAllUsers))
                        {
                            var response = new RequestResponse 
                            {
                                Guid = request.Guid,
                                Head = request.Head,
                                Body = JsonConvert.SerializeObject(db.GetAllUsers().ToList())
                            };

                            var json = JsonConvert.SerializeObject(response);

                            queue.SendMessage(json);
                        }
                    });

                    Console.WriteLine($"02: {message}");
                };
                
                Console.WriteLine("DataService Started");                
                Console.ReadKey();
            }
        }        
    }    
}
