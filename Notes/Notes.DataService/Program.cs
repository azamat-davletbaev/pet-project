using Microsoft.Extensions.DependencyInjection;
using Notes.DataService.Queue;
using Notes.Model;

namespace Notes.DataService
{
    internal class Program
    {
        public static IServiceProvider ServiceProvider = DI.Services.ServiceProvider;        
        static void Main(string[] args)
        {
            using (var ctx = new PostgresContext())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IDataProvider>(z => new DataProvider(ctx));
                serviceCollection.AddSingleton<IQueueSender>(z=> new QueueSender(QueueSettings.QueueName, QueueSettings.HostName, QueueSettings.UserName, QueueSettings.Password));
                serviceCollection.AddSingleton<IQueueReceiver>(z => new QueueReceiver(QueueSettings.QueueName, QueueSettings.HostName, QueueSettings.UserName, QueueSettings.Password));

                ServiceProvider = serviceCollection.BuildServiceProvider();

                //var db = ServiceProvider.GetService<IDataProvider>();
                //var users = db.GetAllUsers();

                var q = ServiceProvider.GetRequiredService<IQueueSender>();                
                q.SendMessage($"Hello !!!");

                var receiver = ServiceProvider.GetRequiredService<IQueueReceiver>();
                receiver.OnReceivered += (message) =>
                {
                    Console.WriteLine($"Получено: {message}");
                };
                                
                Console.ReadKey();
            }
        }
    }
}
