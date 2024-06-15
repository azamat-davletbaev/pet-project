using Microsoft.Extensions.DependencyInjection;
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
                serviceCollection.AddSingleton<IDataProvider>(sp=> new DataProvider(ctx));

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var db = ServiceProvider.GetService<IDataProvider>();
                var users = db.GetAllUsers();
            }
        }
    }
}
