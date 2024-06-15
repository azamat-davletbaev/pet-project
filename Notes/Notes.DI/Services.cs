using Microsoft.Extensions.DependencyInjection;

namespace Notes.DI
{    
    public static class Services
    {
        public static IServiceProvider ServiceProvider { get; private set; }
    }
}
