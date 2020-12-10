using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Erx.Queue
{
    internal class QueueService : IQueueService
    {
        private readonly IModel _channel;
        private readonly RabbitMqOptions _mqOptions;

        public QueueService(IModel channel, RabbitMqOptions mqOptions)
        {
            _channel = channel;
            _mqOptions = mqOptions;
            _channel.QueueDeclare(_mqOptions.QueueName, true, false, false);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(string.Empty, _mqOptions.QueueName, null, body);
        }

        public void ConsumeQueue(Func<string, Task> action = null)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                action?.Invoke(message);
            };
            _channel.BasicConsume(_mqOptions.QueueName, true, consumer);
        }

        public void Dispose() => _channel.Dispose();
    }
}
