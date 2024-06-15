using Microsoft.Extensions.DependencyInjection;
using Notes.DataService.Queue;
using Notes.DI;
using Notes.Model;

namespace Notes.DataService
{
    internal class Program
    {
        public static IServiceProvider ServiceProvider = DI.Services.ServiceProvider;
        private static readonly string MainQueue = "myQueue";
        static void Main(string[] args)
        {
            using (var ctx = new PostgresContext())
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IDataProvider>(z => new DataProvider(ctx));
                serviceCollection.AddSingleton<IQueueSender>(z=> new QueueSender(queueName: MainQueue, hostName: "localhost", userName: "guest", password: "guest"));
                serviceCollection.AddSingleton<IQueueReceiver>(z => new QueueReceiver(queueName: MainQueue, hostName: "localhost", userName: "guest", password: "guest"));

                ServiceProvider = serviceCollection.BuildServiceProvider();

                //var db = ServiceProvider.GetService<IDataProvider>();
                //var users = db.GetAllUsers();

                var q = ServiceProvider.GetRequiredService<IQueueSender>();                
                q.SendMessage($"({DateTime.Now}): Hello !!!");

                var receiver = ServiceProvider.GetRequiredService<IQueueReceiver>();
                receiver.OnReceivered += (message) =>
                {
                    Console.WriteLine($"Получено в {DateTime.Now}: {message}");
                };
                                
                Console.ReadKey();
            }
        }
    }
}
