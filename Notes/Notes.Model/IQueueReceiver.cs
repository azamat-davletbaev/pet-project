using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Notes.Model
{
    public interface IQueueReceiver
    {
        public event Action<string> OnReceivered;
    }
}
