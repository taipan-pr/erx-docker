using System;
using System.Threading.Tasks;

namespace Erx.Queue
{
    public interface IQueueService : IDisposable
    {
        void PublishMessage(string message);
        void ConsumeQueue(Func<string, Task> action = null);
    }
}
