namespace Notes.WebAPI
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(string message);
    }

    public class OrderMessageHandler : IMessageHandler
    {
        public async Task HandleMessageAsync(string message)
        {
            // Логика обработки сообщения типа "Order"

            Console.WriteLine("Received message: {0}", message);
        }
    }
}
