using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Model
{
    public interface IQueue
    {
        public void SendMessage(string Request);
        public event Action<string> OnReceivered;
        //public Task<string> RequestAsync(string reguest, CancellationToken cancellationToken = default);

        
    }

    public class RequestResponse
    {
        public string Guid { get; set; }
        public string Head { get; set; }
        public string Body { get; set; }
    }
}
